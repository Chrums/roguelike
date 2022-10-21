using System;
using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using Fizz6.Collections.Graph;
using Fizz6.Core;
using Fizz6.Geometry;
using Fizz6.Roguelike.World.Region.Zone;
using Newtonsoft.Json;
using UnityEngine;

namespace Fizz6.Roguelike.World.Region
{
    public partial class RegionData
    {
        public class Vertex : GraphExt.IWeighted<Vertex>, IEquatable<Vertex>
        {
            [JsonProperty]
            public Vector2 Position { get; }
            
            [JsonProperty]
            public ZoneType ZoneType { get; }
            
            [JsonProperty]
            public ZoneData ZoneData { get; }

            public Vertex(Vector2 position, ZoneType zoneType)
            {
                Position = position;
                ZoneType = zoneType;
                ZoneData = ZoneData.Generate(0, zoneType, new Vector2Int(4, 4));
            }

            public float Weight(Vertex other) =>
                Vector2.Distance(
                    new Vector2(Position.x, Position.y),
                    new Vector2(other.Position.x, other.Position.y)
                );

            public bool Equals(Vertex other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Position.Equals(other.Position) && ZoneType == other.ZoneType;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Vertex)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Position, (int)ZoneType);
            }
        }

        public static RegionData Generate()
        {
            var regionData = new RegionData();
            
            var types = (ZoneType[])Enum.GetValues(typeof(ZoneType));
            var weights = types.Select(type => RegionConfig.Instance.Zones[type].Weight).ToArray();
            
            var noise = PoissonDisk.Sample(2, RegionConfig.Instance.Radius * 2.0f, RegionConfig.Instance.Distance)
                .Select(array => new Vector2(array[0], array[1]))
                .Where(point => point.magnitude < RegionConfig.Instance.Radius)
                .Select(point => new Point(point.x, point.y) as IPoint)
                .ToArray();
            
            var delaunator = new Delaunator(noise);
            var points = delaunator.Points
                .OrderBy(point => point.Y)
                .ToList();
            
            var startPoint = points.FirstOrDefault();
            var endPoint = points.LastOrDefault();
            
            var startIndex = Array.IndexOf(delaunator.Points, startPoint);
            var endIndex = Array.IndexOf(delaunator.Points, endPoint);

            var vertices = new Dictionary<int, Vertex>();
            var possibilities = new Graph<Vertex>();

            Vertex GetOrAddVertex(int index)
            {
                if (vertices.TryGetValue(index, out var vertex)) return vertex;
                var point = new Vector2((float)delaunator.Points[index].X, (float)delaunator.Points[index].Y);
                
                ZoneType type;
                if (index == startIndex || index == endIndex) type = ZoneType.Town;
                else
                {
                    var typeIndex = weights.WeightedRandom();
                    type = types[typeIndex];
                }
                
                vertex = new Vertex(point, type);
                vertices.Add(index, vertex);
                possibilities.Add(vertex);
                return vertex;
            }
            
            for (var index = 0; index < delaunator.Triangles.Length; index += 3)
            {
                var index0 = delaunator.Triangles[index];
                var vertex0 = GetOrAddVertex(index0);

                var index1 = delaunator.Triangles[index + 1];
                var vertex1 = GetOrAddVertex(index1);

                var index2 = delaunator.Triangles[index + 2];
                var vertex2 = GetOrAddVertex(index2);

                possibilities.Add(vertex0, vertex1);
                possibilities.Add(vertex1, vertex2);
                possibilities.Add(vertex2, vertex0);
            }

            regionData.Start = vertices[startIndex];
            regionData.End = vertices[endIndex];
            regionData.Current = regionData.Start;

            var paths = new List<List<Vertex>>();

            for (var index = 0; index < RegionConfig.Instance.Paths; ++index)
            {
                var path = possibilities.Dijkstra(regionData.Start, regionData.End);
                if (path == null || path.Count < 4) continue;
                paths.Add(path);
                var elimination = UnityEngine.Random.Range(1, path.Count - 1);
                var vertex = path[elimination];
                possibilities.Remove(vertex);
            }
            
            foreach (var path in paths)
            {
                Vertex other = null;
                foreach (var vertex in path)
                {
                    regionData.Graph.Add(vertex);
                    if (other != null) regionData.Graph.Add(other, vertex);
                    other = vertex;
                }
            }

            return regionData;
        }
        
        [JsonProperty]
        public Graph<Vertex> Graph { get; private set; } = new();

        [JsonProperty] 
        public Vertex Start { get; private set; }

        [JsonProperty]
        public Vertex End { get; private set; }
        
        [JsonProperty]
        public Vertex Current { get; set; }
    }
}