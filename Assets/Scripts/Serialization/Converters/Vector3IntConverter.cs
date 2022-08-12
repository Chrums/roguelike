using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Fizz6.Serialization
{
    public class Vector3IntConverter : JsonConverter<Vector3Int>
    {
        public override void WriteJson(JsonWriter writer, Vector3Int value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteValue(value.z);
            writer.WriteEndArray();
        }
    
        public override Vector3Int ReadJson(JsonReader reader, System.Type objectType, Vector3Int existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = hasExistingValue
                ? existingValue
                : Vector3Int.zero;
            
            var array = JArray.Load(reader);
            value.x = array[0].ToObject<int>();
            value.y = array[1].ToObject<int>();
            value.z = array[2].ToObject<int>();

            return value;
        }
    }
}