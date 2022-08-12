using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Fizz6.Serialization
{
    public class Serializer
    {
        private readonly JsonSerializerSettings settings;
        
        public Serializer()
        {
            settings = new JsonSerializerSettings();
            var converters = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(JsonConverter)))
                .Select(Activator.CreateInstance)
                .Cast<JsonConverter>();
            foreach (var converter in converters)
            {
                settings.Converters.Add(converter);
            }

            settings.Formatting = Formatting.Indented;
            settings.TypeNameHandling = TypeNameHandling.All;
        }
        
        public string Serialize(object value) =>
            JsonConvert.SerializeObject(value, settings);

        public T Deserialize<T>(string value) =>
            JsonConvert.DeserializeObject<T>(value, settings);
    }
}