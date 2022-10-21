using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fizz6.Roguelike.World.Region.Zone
{
    [CreateAssetMenu(fileName = "ZoneTileConfig", menuName = "Fizz6/Roguelike/World/Region/Zone/Tile Config")]
    public class ZoneTileConfig : ScriptableObject
    {
        private const string Path = "ZoneTileConfigs";
        private static Dictionary<ZoneType, ZoneTileConfig> _items;
        public static IReadOnlyDictionary<ZoneType, ZoneTileConfig> Items =>
            _items ??= Resources
                .LoadAll<ZoneTileConfig>(Path)
                .ToDictionary(item => item.zoneType, item => item);
        
        [SerializeField]
        private ZoneType zoneType;
    }
}
