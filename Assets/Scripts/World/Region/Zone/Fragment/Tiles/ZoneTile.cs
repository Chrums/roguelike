using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fizz6.Roguelike.World.Region.Zone.Fragment.Tile
{
    [CreateAssetMenu(fileName = "ZoneTile", menuName = "Fizz6/Roguelike/World/Region/Zone/Fragment/Tiles/Zone")]
    public class ZoneTile : UnityEngine.Tilemaps.Tile
    {
        [SerializeField] 
        private Vector2Int spriteLocation;
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            var zoneTileConfig = ZoneTileConfig.Items[Zone.Instance.ZoneData.ZoneType];
        }
    }
}