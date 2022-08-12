using System;
using System.Collections.Generic;
using Fizz6.Roguelike.Input;
using Fizz6.Roguelike.World.Region.Zone;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fizz6.Roguelike.World.Region
{
    public class RegionRenderer
    {

        public event Action<RegionData.Vertex> ClickEvent;

        private readonly Region region;

        public RegionRenderer(Region region)
        {
            this.region = region;
            region.InstantiateVertexEvent += OnInstantiateVertex;
            region.InstantiateEdgeEvent += OnInstantiateEdge;
            region.MoveEvent += OnMove;
        }

        ~RegionRenderer()
        {
            region.InstantiateVertexEvent -= OnInstantiateVertex;
            region.InstantiateEdgeEvent -= OnInstantiateEdge;
            region.MoveEvent -= OnMove;
        }
        
        private void OnMove(RegionVertex from, RegionVertex to)
        {
            if (from != null)
            {
                Apply(from, RegionRendererConfig.Instance.DefaultVertex);
                foreach (var neighborVertex in region.Data.Graph[from.Vertex])
                {
                    var other = region.Vertices[neighborVertex];
                    var edge = region.Edges[(from, other)];
                    Apply(other, RegionRendererConfig.Instance.DefaultVertex);
                    Apply(edge, RegionRendererConfig.Instance.DefaultEdge);
                }
            }
            
            if (to != null)
            {
                Apply(to, RegionRendererConfig.Instance.CurrentVertex);
                foreach (var neighborVertex in region.Data.Graph[to.Vertex])
                {
                    var other = region.Vertices[neighborVertex];
                    var edge = region.Edges[(to, other)];
                    Apply(other, RegionRendererConfig.Instance.OptionVertex);
                    Apply(edge, RegionRendererConfig.Instance.OptionEdge);
                }
            }
        }

        private void Apply(RegionVertex regionVertex, RegionRendererConfig.VertexConfig config)
        {
            var spriteRenderer = regionVertex.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.transform.localScale = Vector3.one * config.Size;
            spriteRenderer.sprite = config.Sprite;
            spriteRenderer.material = config.Material;
            spriteRenderer.color = RegionRendererConfig.Instance.ZoneColors[regionVertex.Vertex.Type];
        }

        private void Apply(RegionEdge regionEdge, RegionRendererConfig.EdgeConfig config)
        {
            var lineRenderer = regionEdge.gameObject.GetComponent<LineRenderer>();
            lineRenderer.material = config.Material;
            lineRenderer.startWidth = config.StartWidth;
            lineRenderer.endWidth = config.EndWidth;
            lineRenderer.startColor = config.StartColor;
            lineRenderer.endColor = config.EndColor;
        }

        private void OnInstantiateVertex(RegionVertex regionVertex)
        {
            regionVertex.gameObject.AddComponent<SpriteRenderer>();
            Apply(regionVertex, RegionRendererConfig.Instance.DefaultVertex);
        }
        
        private void OnInstantiateEdge(RegionEdge regionEdge)
        {
            var lineRenderer = regionEdge.gameObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = false;
            lineRenderer.SetPosition(0, Vector2.zero);
            lineRenderer.SetPosition(1, regionEdge.To.Position - regionEdge.From.Position);
            Apply(regionEdge, RegionRendererConfig.Instance.DefaultEdge);
        }
    }
}