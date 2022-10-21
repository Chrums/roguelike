using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Collections.Graph;
using Fizz6.Core;
using Fizz6.Roguelike.Spawner;
using Fizz6.Roguelike.World.Region.Zone.Fragment.Tile;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fizz6.Roguelike.World.Region.Zone
{
    public partial class ZoneData
    {
        private class FragmentVertex
        {
            public Vector3Int FragmentOffset { get; }
            
            private Fragment.Fragment fragment;
            public Fragment.Fragment Fragment
            {
                get => fragment;
                set
                {
                    fragment = value;
                    
                    if (!fragment.LinkTiles.Random(out var entrance)) 
                        throw new Exception($"{value.name} must contain at least one {nameof(LinkTile)}");
                
                    localEntranceTileCell = entrance;
                    localExitTileCell = fragment.LinkTiles.Except(new[] { entrance }).Random(out var exit)
                        ? exit
                        : entrance;
                }
            }
            
            public Vector3Int FragmentCenterTileCellOffset { get; set; }

            public Vector3Int TileCellOffset => Vector3Int.RoundToInt(FragmentCenterTileCellOffset - Fragment.Tilemap.cellBounds.center);
            
            private Vector3Int localEntranceTileCell;
            public Vector3Int EntranceTileCell => localEntranceTileCell + TileCellOffset;
            
            private Vector3Int localExitTileCell;
            public Vector3Int ExitTileCell => localExitTileCell + TileCellOffset;

            public FragmentVertex(Vector3Int fragmentOffset) => FragmentOffset = fragmentOffset;
        }

        private class TileVertex
        {
            public Vector3Int Position { get; }

            public TileVertex(Vector3Int position) => Position = position;
        }

        public static ZoneData Generate(int level, ZoneType zoneType, Vector2Int dimensions)
        {
            var zoneData = new ZoneData
            {
                Level = level,
                ZoneType = zoneType
            };
            
            // Generate a Graph of Fragments whose connections represent a perfect maze
            var fragmentGraph = GraphExt.RecursiveBacktrack(
                new Vector3Int(dimensions.x, dimensions.y, 1),
                out var fragmentGrid,
                position => new FragmentVertex(position)
            );

            // Keep track of which Fragments we have used so we can have more variety
            var fragments = new List<Fragment.Fragment>();

            // Assign a Fragment to each FragmentVertex in the grid
            for (var x = 0; x < dimensions.x; ++x)
            for (var y = 0; y < dimensions.y; ++y)
            {
                var fragmentVertex = fragmentGrid[x, y, 0];
                var fragment = Fragment.Fragment.Random(fragments);
                
                // Reset our list of Fragments if we've used all Fragments
                if (fragment == null)
                {
                    fragments.Clear();
                    fragment = Fragment.Fragment.Random(fragments);
                }
                
                fragments.Add(fragment);
                fragment.Tilemap.CompressBounds();
                fragmentVertex.Fragment = fragment;
            }

            // Size in number of Tiles of each column in the Fragment grid
            var columnSizes = new int[dimensions.x];
            
            // Size in number of Tiles of each row in the Fragment grid
            var rowSizes = new int[dimensions.y];
            
            // Determine the layout of the grid based on the largest Fragment in each column/row
            for (var x = 0; x < dimensions.x; ++x)
            for (var y = 0; y < dimensions.y; ++y)
            {
                var fragmentVertex = fragmentGrid[x, y, 0];
                var cellBounds = fragmentVertex.Fragment.Tilemap.cellBounds;
                columnSizes[x] = Math.Max(cellBounds.size.x + ZoneConfig.Instance.FragmentSpacing * 2, columnSizes[x]);
                rowSizes[y] = Math.Max(cellBounds.size.y + ZoneConfig.Instance.FragmentSpacing * 2, rowSizes[y]);
            }

            // Calculate the dimensions of the Zone in terms of Tiles
            var sizes = new Vector2Int(
                columnSizes.Sum(),
                rowSizes.Sum()
            );

            zoneData.Tiles = new TileBase[sizes.x, sizes.y];
            
            // Determine the center of each cell and copy the Fragment's Tiles into the ZoneData
            var offset = Vector2Int.zero;
            for (var x = 0; x < dimensions.x; ++x)
            {
                var columnSize = columnSizes[x];
                
                for (var y = 0; y < dimensions.y; ++y)
                {
                    var rowSize = rowSizes[y];

                    var fragmentVertex = fragmentGrid[x, y, 0];
                    fragmentVertex.FragmentCenterTileCellOffset = new Vector3Int(
                        offset.x + columnSize / 2,
                        offset.y + rowSize / 2,
                        0
                    );

                    var tilemapCellBounds = fragmentVertex.Fragment.Tilemap.cellBounds;
                    for (var tilemapCellX = tilemapCellBounds.xMin; tilemapCellX <= tilemapCellBounds.xMax; ++tilemapCellX)
                    for (var tilemapCellY = tilemapCellBounds.yMin; tilemapCellY <= tilemapCellBounds.yMax; ++tilemapCellY)
                    {
                        var position = new Vector3Int(tilemapCellX, tilemapCellY, 0);
                        var tile = fragmentVertex.Fragment.Tilemap.GetTile(position);
                        if (tile == null || tile is LinkTile) continue;

                        var cellX = fragmentVertex.TileCellOffset.x + tilemapCellX;
                        var cellY = fragmentVertex.TileCellOffset.y + tilemapCellY;
                        if (cellX < 0 || cellX >= zoneData.Tiles.GetLength(0) ||
                            cellY < 0 || cellY >= zoneData.Tiles.GetLength(1))
                            continue;
                
                        zoneData.Tiles[cellX, cellY] = tile;
                    }
                    
                    offset.y += rowSize;
                }

                offset.y = 0;
                offset.x += columnSize;
            }

            // Generate a Graph of cells that represent each Tile in the Zone
            var pathGraph = GraphExt.Grid(
                new Vector3Int(sizes.x, sizes.y, 1),
                out var pathGrid,
                position => new TileVertex(position)
            );
            
            // Connect all cells in the Graph to all adjacent empty cells
            for (var x = 0; x < zoneData.Tiles.GetLength(0); ++x)
            for (var y = 0; y < zoneData.Tiles.GetLength(1); ++y)
            {
                var tileVertex = pathGrid[x, y, 0];
                foreach (var direction in GraphExt.Directions)
                {
                    if (direction.z != 0) continue;
                    var otherPosition = new Vector2Int(x + direction.x, y + direction.y);
                    if (otherPosition.x < 0 || otherPosition.x >= sizes.x || 
                        otherPosition.y < 0 || otherPosition.y >= sizes.y) continue;
                    var otherTile = zoneData.Tiles[otherPosition.x, otherPosition.y];
                    if (otherTile != null) continue;
                    var otherVertex = pathGrid[otherPosition.x, otherPosition.y, 0];
                    pathGraph.Add(tileVertex, otherVertex);
                }
            }

            // Create a list of connections between Fragments that represent the maze
            var fragmentConnections = new List<(FragmentVertex, FragmentVertex)>();
            for (var x = 0; x < dimensions.x; ++x)
            for (var y = 0; y < dimensions.y; ++y)
            {
                var fragmentVertex = fragmentGrid[x, y, 0];
                fragmentConnections.AddRange(fragmentGraph[fragmentVertex].Select(otherVertex => (fragmentVertex, otherVertex)));
            }

            // Calculate a path on the Tile Graph between each of the connected Fragments
            foreach (var (fromFragmentVertex, toFragmentVertex) in fragmentConnections)
            {
                var fromPosition = fromFragmentVertex.ExitTileCell;
                var fromVertex = pathGrid[fromPosition.x, fromPosition.y, fromPosition.z];
                var toPosition = toFragmentVertex.EntranceTileCell;
                var toVertex = pathGrid[toPosition.x, toPosition.y, toPosition.z];
                var path = pathGraph.BreadthFirstSearch(fromVertex, toVertex);
                
                // Connect the Fragments with Tiles and expand them based on PathWeight
                for (var weight = 0; weight < ZoneConfig.Instance.PathWeight; ++weight)
                {
                    var neighbors = new List<TileVertex>();
                    
                    foreach (var tileVertex in path)
                    {
                        zoneData.Tiles[tileVertex.Position.x, tileVertex.Position.y] = ZoneConfig.Instance.GroundTile;
                        neighbors.AddRange(
                            from direction in GraphExt.Directions where direction.z == 0 
                            select tileVertex.Position + direction into otherPosition 
                            where otherPosition.x >= 0 && 
                                  otherPosition.x < sizes.x && 
                                  otherPosition.y >= 0 && 
                                  otherPosition.y < sizes.y && 
                                  zoneData.Tiles[otherPosition.x, otherPosition.y] == null 
                            select pathGrid[otherPosition.x, otherPosition.y, 0]
                        );
                    }

                    path = neighbors;
                }
            }
            
            // Fill remaining Tiles with unpassable terrain
            for (var x = 0; x < zoneData.Tiles.GetLength(0); ++x)
            for (var y = 0; y < zoneData.Tiles.GetLength(1); ++y)
            {
                if (zoneData.Tiles[x, y] != null) continue;
                zoneData.Tiles[x, y] = ZoneConfig.Instance.TreeTile;
            }
            
            for (var x = 0; x < dimensions.x; ++x)
            for (var y = 0; y < dimensions.y; ++y)
            {
                var fragmentVertex = fragmentGrid[x, y, 0];
                zoneData.Spawnables = fragmentVertex.Fragment.Spawn(zoneData).ToList();
                
                foreach (var spawnable in zoneData.Spawnables)
                {
                    Matrix4x4.Translate(fragmentVertex.FragmentCenterTileCellOffset);
                }
            }

            return zoneData;
        }

        [JsonProperty]
        public int Level { get; private set; }
        
        [JsonProperty]
        public ZoneType ZoneType { get; private set; }
        
        [JsonProperty]
        public TileBase[,] Tiles { get; private set; }

        [JsonProperty]
        public List<ISpawnable> Spawnables { get; private set; }
    }
}