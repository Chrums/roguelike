using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Core;
using Fizz6.Roguelike.World.Region.Zone;
using UnityEngine;

namespace Fizz6.Roguelike.World.Region
{
    [CreateAssetMenu(fileName = "RegionRendererConfig", menuName = "Fizz6/Roguelike/World/Region/Renderer Config")]
    public class RegionRendererConfig : SingletonScriptableObject<RegionRendererConfig>
    {
        [Serializable]
        public class VertexConfig
        {
            [SerializeField] 
            private Sprite sprite;
            public Sprite Sprite => sprite;

            [SerializeField] 
            private Material material;
            public Material Material => material;

            [SerializeField] 
            private float size;
            public float Size => size;
        }
        
        [Serializable]
        public class EdgeConfig
        {
        
            [SerializeField] 
            private Material material;
            public Material Material => material;

            [SerializeField]
            private float startWidth;
            public float StartWidth => startWidth;

            [SerializeField]
            private float endWidth;
            public float EndWidth => endWidth;

            [SerializeField]
            private Color startColor;
            public Color StartColor => startColor;

            [SerializeField]
            private Color endColor;
            public Color EndColor => endColor;
        }

        [Serializable]
        private class ZoneData
        {
            [SerializeField]
            private ZoneType type;
            public ZoneType Type => type;

            [SerializeField] 
            private Color color;
            public Color Color => color;
        }

        [SerializeField]
        private VertexConfig defaultVertex;
        public VertexConfig DefaultVertex => defaultVertex;

        [SerializeField]
        private VertexConfig currentVertex;
        public VertexConfig CurrentVertex => currentVertex;

        [SerializeField]
        private VertexConfig optionVertex;
        public VertexConfig OptionVertex => optionVertex;

        [SerializeField]
        private EdgeConfig defaultEdge;
        public EdgeConfig DefaultEdge => defaultEdge;

        [SerializeField]
        private EdgeConfig optionEdge;
        public EdgeConfig OptionEdge => optionEdge;

        [SerializeField]
        private List<ZoneData> zoneData;

        private Dictionary<ZoneType, Color> zoneColors;
        public Dictionary<ZoneType, Color> ZoneColors =>
            zoneColors ??= zoneData
                .ToDictionary(worldNodeColor => worldNodeColor.Type, worldNodeColor => worldNodeColor.Color);
    }
}