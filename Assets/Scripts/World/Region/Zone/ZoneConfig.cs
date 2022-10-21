using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Core;
using Fizz6.Roguelike.World.Region.Zone;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Fizz6.Roguelike.World.Region.Zone
{
    [CreateAssetMenu(fileName = "ZoneConfig", menuName = "Fizz6/Roguelike/World/Region/Zone/Config")]
    public class ZoneConfig : SingletonScriptableObject<ZoneConfig>
    {
        [SerializeField]
        private int fragmentSpacing = 2;
        public int FragmentSpacing => fragmentSpacing;

        [SerializeField]
        private int pathWeight = 2;
        public int PathWeight => pathWeight;

        [SerializeField] 
        private Tile groundTile;
        public Tile GroundTile => groundTile;

        [SerializeField] 
        private Tile treeTile;
        public Tile TreeTile => treeTile;
    }
}