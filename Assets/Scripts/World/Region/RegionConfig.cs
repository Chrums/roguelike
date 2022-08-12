using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Core;
using Fizz6.Roguelike.World.Region.Zone;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fizz6.Roguelike.World.Region
{
    [CreateAssetMenu(fileName = "RegionConfig", menuName = "Fizz6/Roguelike/World/Region/Config")]
    public class RegionConfig : SingletonScriptableObject<RegionConfig>
    {
        [SerializeField]
        private float radius = 8.0f;
        public float Radius => radius;
        
        [SerializeField]
        private float distance = 1.0f;
        public float Distance => distance;

        [SerializeField] 
        private int paths = 32;
        public int Paths => paths;

        [Serializable]
        public class ZoneData
        {
            [SerializeField]
            private ZoneType type;
            public ZoneType Type => type;

            [SerializeField]
            private float weight;
            public float Weight => weight;
        }

        [SerializeField]
        private List<ZoneData> zoneData;

        private Dictionary<ZoneType, ZoneData> zones;
        public Dictionary<ZoneType, ZoneData> Zones =>
            zones ??= zoneData
                .ToDictionary(_ => _.Type, _ => _);
    }
}