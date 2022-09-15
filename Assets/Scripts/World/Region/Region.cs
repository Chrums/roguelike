using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Roguelike.Input;
using Fizz6.Serialization;
using UnityEngine;

namespace Fizz6.Roguelike.World.Region
{
    public class Region : MonoBehaviour
    {
        private const string VertexContainerName = "Vertices";
        private const string VertexName = "Vertex";
        private const string EdgeContainerName = "Edges";
        private const string EdgeName = "Edge";
        
        public RegionData Data { get; private set; }
        
        private RegionRenderer regionRenderer;

        private GameObject vertexContainer;
        private readonly Dictionary<RegionData.Vertex, GameObject> vertices = new();
        public IReadOnlyDictionary<RegionData.Vertex, GameObject> Vertices => vertices;

        private GameObject edgeContainer;
        private readonly Dictionary<(RegionData.Vertex, RegionData.Vertex), GameObject> edges = new();
        public IReadOnlyDictionary<(RegionData.Vertex, RegionData.Vertex), GameObject> Edges => edges;
        
        public RegionData.Vertex CurrentVertex { get; private set; }

        public event Action<RegionData.Vertex, GameObject> InstantiateVertexEvent;
        public event Action<RegionData.Vertex, RegionData.Vertex, GameObject> InstantiateEdgeEvent;
        public event Action<RegionData.Vertex, RegionData.Vertex> MoveEvent;
        
        private void Awake()
        {
            Data = new RegionData();
            regionRenderer = new RegionRenderer(this);
            
            vertexContainer = new GameObject(VertexContainerName)
            {
                transform = { parent = transform }
            };
            
            edgeContainer = new GameObject(EdgeContainerName)
            {
                transform = { parent = transform }
            };
            
            foreach (var vertex in Data.Graph.Vertices)
                Instantiate(vertex);
            
            foreach (var vertex in Data.Graph.Vertices)
                foreach (var edge in Data.Graph[vertex])
                    Instantiate(vertex, edge);

            CurrentVertex = Data.Start;
            
            MoveEvent?.Invoke(null, CurrentVertex);

            var serializer = new Serializer();
            var json = serializer.Serialize(Data);
            Data = serializer.Deserialize<RegionData>(json);
        }
        
        private void Instantiate(RegionData.Vertex vertex)
        {
            var vertexGameObject = new GameObject(VertexName)
            {
                transform =
                {
                    parent = vertexContainer.transform,
                    localPosition = vertex.Position
                }
            };
            
            vertices.Add(vertex, vertexGameObject);
            
            var vertexCollider = vertexGameObject.AddComponent<CircleCollider2D>();
            vertexCollider.radius = 1.0f;
            var mouseEventListener = vertexGameObject.AddComponent<MouseEventListener>();
            void OnClick() => Move(vertex);
            mouseEventListener.MouseDownEvent += OnClick;
            
            InstantiateVertexEvent?.Invoke(vertex, vertexGameObject);
        }
        
        private void Instantiate(RegionData.Vertex from, RegionData.Vertex to)
        {
            var edgeGameObject = new GameObject(EdgeName)
            {
                transform =
                {
                    parent = edgeContainer.transform,
                    localPosition = from.Position
                }
            };
            
            edges.Add((from, to), edgeGameObject);
            
            InstantiateEdgeEvent?.Invoke(from, to, edgeGameObject);
        }

        private void Move(RegionData.Vertex target)
        {
            var current = CurrentVertex;
            if (!Data.Graph[CurrentVertex].Contains(target)) return;
            CurrentVertex = target;
            MoveEvent?.Invoke(current, target);
        }
    }
}