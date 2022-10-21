using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fizz6.Serialization
{
    public class TileConverter : JsonConverter<TileBase>
    {
        private const string Path = "Tiles";
        private static Dictionary<string, TileBase> _items;
        private static IReadOnlyDictionary<string, TileBase> Items =>
            _items ??= Resources
                .LoadAll<TileBase>(Path)
                .ToDictionary(tile => tile.name, tile => tile);
        
        public override void WriteJson(JsonWriter writer, TileBase value, JsonSerializer serializer)
        {
            var name = value.name;
            writer.WriteValue(name);
        }

        public override TileBase ReadJson(JsonReader reader, System.Type objectType, TileBase existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            if (value == null) return existingValue;
            var name = value.ToString();
            return name != null 
                ? Items[name]
                : existingValue;
        }
    }
}