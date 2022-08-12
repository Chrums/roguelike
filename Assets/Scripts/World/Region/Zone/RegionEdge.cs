using UnityEngine;

namespace Fizz6.Roguelike.World.Region.Zone
{
    public class RegionEdge : MonoBehaviour
    {
        public RegionData.Vertex From { get; set; }
        public RegionData.Vertex To { get; set; }
    }
}