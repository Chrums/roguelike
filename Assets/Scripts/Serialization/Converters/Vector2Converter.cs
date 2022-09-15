using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Fizz6.Serialization
{
    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteEndArray();
        }
    
        public override Vector2 ReadJson(JsonReader reader, System.Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = hasExistingValue
                ? existingValue
                : Vector2.zero;
            
            var array = JArray.Load(reader);
            value.x = array[0].ToObject<float>();
            value.y = array[1].ToObject<float>();

            return value;
        }
    }
}