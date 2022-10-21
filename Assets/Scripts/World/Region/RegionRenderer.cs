using System;
using UnityEngine;

namespace Fizz6.Roguelike.World.Region
{
    public class RegionRenderer
    {
        private readonly Region region;

        private RegionData.Vertex currentVertex;
        private Region.Control currentState;

        public RegionRenderer(Region region)
        {
            this.region = region;
            region.InstantiateVertexEvent += OnInstantiateVertex;
            region.InstantiateEdgeEvent += OnInstantiateEdge;
            region.UpdateEvent += OnUpdate;
            region.MoveEvent += OnMove;
        }

        ~RegionRenderer()
        {
            region.InstantiateVertexEvent -= OnInstantiateVertex;
            region.InstantiateEdgeEvent -= OnInstantiateEdge;
            region.UpdateEvent -= OnUpdate;
            region.MoveEvent -= OnMove;
        }

        private void OnUpdate()
        {
            if (region.State == currentState && region.RegionData.Current.Equals(currentVertex)) return;
            
            if (!currentVertex.Equals(region.RegionData.Current))
            {
                GameObject vertexGameObject;
                
                if (currentVertex != null)
                {
                    vertexGameObject = region.Vertices[currentVertex];
                    Apply(currentVertex, vertexGameObject, RegionRendererConfig.Instance.DefaultVertex);
                }

                currentVertex = region.RegionData.Current;

                vertexGameObject = region.Vertices[currentVertex];
                Apply(currentVertex, vertexGameObject, RegionRendererConfig.Instance.CurrentVertex);
            }

            if (currentState != region.State)
            {
                
            }
        }
        
        private void OnMove(RegionData.Vertex from, RegionData.Vertex to)
        {
            if (from != null)
            {
                var vertexGameObject = region.Vertices[from];
                Apply(from, vertexGameObject, RegionRendererConfig.Instance.DefaultVertex);
                foreach (var vertex in region.RegionData.Graph[from])
                {
                    var option = region.Vertices[vertex];
                    var edge = region.Edges[(from, vertex)];
                    Apply(vertex, option, RegionRendererConfig.Instance.DefaultVertex);
                    Apply(edge, RegionRendererConfig.Instance.DefaultEdge);
                }
            }
            
            if (to != null)
            {
                var vertexGameObject = region.Vertices[to];
                Apply(to, vertexGameObject, RegionRendererConfig.Instance.CurrentVertex);
                foreach (var vertex in region.RegionData.Graph[to])
                {
                    var option = region.Vertices[vertex];
                    var edge = region.Edges[(to, vertex)];
                    Apply(vertex, option, RegionRendererConfig.Instance.OptionVertex);
                    Apply(edge, RegionRendererConfig.Instance.OptionEdge);
                }
            }
        }

        private void Apply(RegionData.Vertex vertex, GameObject vertexGameObject, RegionRendererConfig.VertexConfig config)
        {
            var spriteRenderer = vertexGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.transform.localScale = Vector3.one * config.Size;
            spriteRenderer.sprite = config.Sprite;
            spriteRenderer.material = config.Material;
            spriteRenderer.color = RegionRendererConfig.Instance.ZoneColors[vertex.ZoneType];
        }

        private void Apply(GameObject edgeGameObject, RegionRendererConfig.EdgeConfig config)
        {
            var lineRenderer = edgeGameObject.GetComponent<LineRenderer>();
            lineRenderer.material = config.Material;
            lineRenderer.startWidth = config.StartWidth;
            lineRenderer.endWidth = config.EndWidth;
            lineRenderer.startColor = config.StartColor;
            lineRenderer.endColor = config.EndColor;
        }

        private void OnInstantiateVertex(RegionData.Vertex vertex, GameObject vertexGameObject)
        {
            vertexGameObject.AddComponent<SpriteRenderer>();
            Apply(vertex, vertexGameObject, RegionRendererConfig.Instance.DefaultVertex);
        }
        
        private void OnInstantiateEdge(RegionData.Vertex from, RegionData.Vertex to, GameObject edgeGameObject)
        {
            var lineRenderer = edgeGameObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = false;
            lineRenderer.SetPosition(0, Vector2.zero);
            lineRenderer.SetPosition(1, to.Position - from.Position);
            Apply(edgeGameObject, RegionRendererConfig.Instance.DefaultEdge);
        }
    }
}