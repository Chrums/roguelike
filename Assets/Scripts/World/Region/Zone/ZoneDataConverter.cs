using System;
using System.Linq;
using Fizz6.Collections.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fizz6.Roguelike.World.Region.Zone
{
    public partial class ZoneData
    {
        public class ZoneDataConverter : JsonConverter<ZoneData>
        {
            public override bool CanWrite => false;
            public override void WriteJson(JsonWriter writer, ZoneData value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override ZoneData ReadJson(JsonReader reader, System.Type objectType, ZoneData existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var value = hasExistingValue
                    ? existingValue
                    : new ZoneData();

                var data = JObject.Load(reader);
                
                // var graph = data[nameof(Graph)]?.ToObject<Graph<Vertex>>(serializer);
                // var start = data[nameof(Start)]?.ToObject<Vertex>(serializer);;
                // var end = data[nameof(End)]?.ToObject<Vertex>(serializer);
                // var current = data[nameof(Current)]?.ToObject<Vertex>(serializer);
                //
                // if (graph == null ||
                //     start == null ||
                //     end == null ||
                //     current == null) return value;
                //
                // value.Graph = graph;
                // value.Start = value.Graph.Vertices.FirstOrDefault(vertex => vertex.Position == start.Position);
                // value.End = value.Graph.Vertices.FirstOrDefault(vertex => vertex.Position == end.Position);
                // value.Current = value.Graph.Vertices.FirstOrDefault(vertex => vertex.Position == current.Position);

                return value;
            }
        }
    }
}