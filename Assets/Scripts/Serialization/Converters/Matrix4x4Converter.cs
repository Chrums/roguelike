using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Fizz6.Serialization
{
    public class Matrix4x4Converter : JsonConverter<Matrix4x4>
    {
        public override void WriteJson(JsonWriter writer, Matrix4x4 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            
            for (var index = 0; index < 16; ++index)
            {
                writer.WriteValue(value[index]);
            }
            
            writer.WriteEndArray();
        }
    
        public override Matrix4x4 ReadJson(JsonReader reader, System.Type objectType, Matrix4x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = hasExistingValue
                ? existingValue
                : Matrix4x4.identity;
            
            var array = JArray.Load(reader);
            
            for (var index = 0; index < 16; ++index)
            {
                value[index] = array[index].ToObject<float>();
            }

            return value;
        }
    }
}