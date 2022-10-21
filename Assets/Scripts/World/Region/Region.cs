using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fizz6.Roguelike.Input;
using UnityEngine;

namespace Fizz6.Roguelike.World.Region
{
    public class Region : SingletonMonoBehaviour<Region>
    {
        private const string VertexContainerName = "Vertices";
        private const string VertexName = "Vertex";
        private const string EdgeContainerName = "Edges";
        private const string EdgeName = "Edge";

        public enum Control
        {
            Default,
            Move
        }

        public RegionData RegionData { get; private set; }

        private Control state;

        public Control State
        {
            get => state;
            set
            {
                state = value;
                UpdateEvent?.Invoke();
            }
        }
        
        private RegionRenderer regionRenderer;

        private GameObject vertexContainer;
        private readonly Dictionary<RegionData.Vertex, GameObject> vertices = new();
        public IReadOnlyDictionary<RegionData.Vertex, GameObject> Vertices => vertices;

        private GameObject edgeContainer;
        private readonly Dictionary<(RegionData.Vertex, RegionData.Vertex), GameObject> edges = new();
        public IReadOnlyDictionary<(RegionData.Vertex, RegionData.Vertex), GameObject> Edges => edges;
        
        public event Action<RegionData.Vertex, GameObject> InstantiateVertexEvent;
        public event Action<RegionData.Vertex, RegionData.Vertex, GameObject> InstantiateEdgeEvent;
        public event Action UpdateEvent;
        public event Action<RegionData.Vertex, RegionData.Vertex> MoveEvent;

        public void Load(RegionData regionData = null)
        {
            if (vertexContainer) Destroy(vertexContainer);
            if (edgeContainer) Destroy(edgeContainer);

            RegionData = regionData ?? RegionData.Generate();
            
            vertexContainer = new GameObject(VertexContainerName)
            {
                transform = { parent = transform },
                hideFlags = HideFlags.NotEditable | 
                            HideFlags.DontSave
            };
            
            edgeContainer = new GameObject(EdgeContainerName)
            {
                transform = { parent = transform },
                hideFlags = HideFlags.NotEditable | 
                            HideFlags.DontSave
            };
            
            foreach (var vertex in RegionData.Graph.Vertices)
                Instantiate(vertex);
            
            foreach (var vertex in RegionData.Graph.Vertices)
                foreach (var edge in RegionData.Graph[vertex])
                    Instantiate(vertex, edge);
            
            MoveEvent?.Invoke(null, RegionData.Current);
        }

        protected override void Awake()
        {
            base.Awake();
            regionRenderer = new RegionRenderer(this);
        }

        private void Instantiate(RegionData.Vertex vertex)
        {
            var vertexGameObject = new GameObject(VertexName)
            {
                transform =
                {
                    parent = vertexContainer.transform,
                    localPosition = vertex.Position,
                    hideFlags = HideFlags.NotEditable | 
                                HideFlags.DontSave
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
                    localPosition = from.Position,
                    hideFlags = HideFlags.NotEditable | 
                                HideFlags.DontSave
                }
            };
            
            edges.Add((from, to), edgeGameObject);
            
            InstantiateEdgeEvent?.Invoke(from, to, edgeGameObject);
        }

        private void Move(RegionData.Vertex target)
        {
            var current = RegionData.Current;
            if (!RegionData.Graph[RegionData.Current].Contains(target)) return;
            RegionData.Current = target;
            MoveEvent?.Invoke(current, target);
        }
    }
}