using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fizz6.Serialization
{
    public class TilemapConverter : JsonConverter<Tilemap>
    {
        public override void WriteJson(JsonWriter writer, Tilemap value, JsonSerializer serializer)
        {
            var bounds = value.cellBounds;
            
            writer.WriteStartArray();
                    
            for (var z = bounds.zMin; z < bounds.zMax; ++z)
            {
                writer.WriteStartArray();
                
                for (var y = bounds.yMin; y < bounds.yMax; ++y)
                {
            
                    writer.WriteStartArray();
            
                    for (var x = bounds.xMin; x < bounds.xMax; ++x)
                    {
                        var position = new Vector3Int(x, y, z);
                        var tile = value.GetTile(position);
                        serializer.Serialize(writer, tile);
                    }
            
                    writer.WriteEndArray();
                }
                
                writer.WriteEndArray();
            }
                    
            writer.WriteEndArray();
        }
    
        public override Tilemap ReadJson(JsonReader reader, System.Type objectType, Tilemap existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = hasExistingValue
                ? existingValue
                : null;

            if (value == null) return existingValue;
            
            var zArray = JArray.Load(reader);
            for (var z = 0; z < zArray.Count; ++z)
            {
                var yArray = (JArray)zArray[z];
                for (var y = 0; y < yArray.Count; ++y)
                {
                    var xArray = (JArray)yArray[y];
                    for (var x = 0; x < zArray.Count; ++x)
                    {
                        var token = xArray[x];
                        var position = new Vector3Int(x, y, z);
                        var tile = token.ToObject<Tile>(serializer);
                        value.SetTile(position, tile);
                    }
                }
            }

            return value;
        }
    }
}