using System;
using System.Linq;
using Fizz6.Collections.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fizz6.Roguelike.World.Region
{
    public partial class RegionData
    {
        public class RegionDataConverter : JsonConverter<RegionData>
        {
            public override bool CanWrite => false;
            public override void WriteJson(JsonWriter writer, RegionData value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override RegionData ReadJson(JsonReader reader, System.Type objectType, RegionData existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var value = hasExistingValue
                    ? existingValue
                    : new RegionData();

                var data = JObject.Load(reader);
                
                var graph = data[nameof(Graph)]?.ToObject<Graph<Vertex>>(serializer);
                var start = data[nameof(Start)]?.ToObject<Vertex>(serializer);;
                var end = data[nameof(End)]?.ToObject<Vertex>(serializer);
                var current = data[nameof(Current)]?.ToObject<Vertex>(serializer);
                
                if (graph == null ||
                    start == null ||
                    end == null ||
                    current == null) return value;
                
                value.Graph = graph;
                value.Start = value.Graph.Vertices.FirstOrDefault(vertex => vertex.Position == start.Position);
                value.End = value.Graph.Vertices.FirstOrDefault(vertex => vertex.Position == end.Position);
                value.Current = value.Graph.Vertices.FirstOrDefault(vertex => vertex.Position == current.Position);

                return value;
            }
        }
    }
}