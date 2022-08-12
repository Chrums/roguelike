using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Roguelike.Input;
using Fizz6.Roguelike.World.Region.Zone;
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
        private readonly Dictionary<RegionData.Vertex, RegionVertex> vertices = new();
        public IReadOnlyDictionary<RegionData.Vertex, RegionVertex> Vertices => vertices;

        private GameObject edgeContainer;
        private readonly Dictionary<(RegionVertex, RegionVertex), RegionEdge> edges = new();
        public IReadOnlyDictionary<(RegionVertex, RegionVertex), RegionEdge> Edges => edges;
        
        public RegionVertex CurrentVertex { get; private set; }

        public event Action<RegionVertex> InstantiateVertexEvent;
        public event Action<RegionEdge> InstantiateEdgeEvent;
        public event Action<RegionVertex, RegionVertex> MoveEvent;
        
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

            CurrentVertex = Vertices[Data.Start];
            MoveEvent?.Invoke(null, CurrentVertex);
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
            
            var regionVertex = vertexGameObject.AddComponent<RegionVertex>();
            regionVertex.Vertex = vertex;
            vertices.Add(vertex, regionVertex);
            
            var collider = vertexGameObject.AddComponent<CircleCollider2D>();
            collider.radius = 1.0f;
            var mouseEventListener = vertexGameObject.AddComponent<MouseEventListener>();
            void OnClick() => Move(regionVertex);
            mouseEventListener.MouseDownEvent += OnClick;
            
            InstantiateVertexEvent?.Invoke(regionVertex);
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

            var regionEdge = edgeGameObject.AddComponent<RegionEdge>();
            regionEdge.From = from;
            regionEdge.To = to;
            edges.Add((vertices[from], vertices[to]), regionEdge);
            vertices[from].Edges.Add(regionEdge);
            
            InstantiateEdgeEvent?.Invoke(regionEdge);
        }

        private void Move(RegionVertex target)
        {
            var current = CurrentVertex;
            if (!Data.Graph[CurrentVertex.Vertex].Contains(target.Vertex)) return;
            CurrentVertex = target;
            MoveEvent?.Invoke(current, target);
        }
    }
}