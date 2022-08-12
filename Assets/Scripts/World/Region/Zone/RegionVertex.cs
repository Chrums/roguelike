using System.Collections.Generic;
using UnityEngine;

namespace Fizz6.Roguelike.World.Region.Zone
{
    public class RegionVertex : MonoBehaviour
    {
        public RegionData.Vertex Vertex { get; set; }
        public readonly List<RegionEdge> Edges = new();
    }
}
