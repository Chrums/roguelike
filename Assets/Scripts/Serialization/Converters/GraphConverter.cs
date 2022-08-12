using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Collections.Graph;
using Newtonsoft.Json;

namespace Fizz6.Roguelike.Serialization
{
    public class GraphConverter : JsonConverter
    {
        private class Node
        {
            public object Item;
            public readonly List<int> Edges = new();
        }

        public override bool CanConvert(System.Type objectType) =>
            objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(Graph<>) ||
                                         objectType.GetGenericTypeDefinition() == typeof(IGraph));

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!CanConvert(value.GetType()) || value is not IGraph graph) 
                throw new Exception($"This {nameof(JsonConverter)} is not for {value.GetType()}.");
            
            var indices = new Dictionary<object, int>();
            var items = graph.Items.ToArray();
            for (var index = 0; index < items.Length; ++index)
            {
                var item = items[index];
                indices[item] = index;
            }
            
            var nodes = graph.Items
                .Select(item => new Node { Item = item })
                .ToArray();

            for (var index = 0; index < items.Length; ++index)
            {
                var item = items[index];
                var node = nodes[index];
                node.Edges.AddRange(graph[item].Select(edge => indices[edge]));
            }
            
            serializer.Serialize(writer, nodes);
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!CanConvert(objectType))
                throw new Exception($"This {nameof(JsonConverter)} is not for {objectType}.");
            
            var vertexType = objectType.GetGenericArguments()[0];
            var graphType = typeof(Graph<>).MakeGenericType(vertexType);

            var value = existingValue ?? Activator.CreateInstance(graphType);
            if (value is not IGraph graph) return existingValue;

            var nodes = serializer.Deserialize<Node[]>(reader);
            if (nodes == null) return existingValue;
            
            foreach (var node in nodes)
            {
                graph.Add(node.Item);
            }

            foreach (var node in nodes)
                foreach (var edge in node.Edges) 
                    graph.Add(node.Item, nodes[edge].Item);

            return value;
        }
    }
}