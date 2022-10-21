using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fizz6.Roguelike.World.Region.Zone.Fragment.Tile
{
    [CreateAssetMenu(fileName = "GroundTile", menuName = "Fizz6/Roguelike/World/Region/Zone/Fragment/Tiles/Ground")]
    public class GroundTile : UnityEngine.Tilemaps.Tile
    {
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            // var zoneTileConfig = ZoneTileConfig.Items[Zone.Instance.ZoneData.ZoneType];
        }
    }
}