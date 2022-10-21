using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Fizz6.Roguelike.World.Region.Zone
{
    public class Zone : SingletonMonoBehaviour<Zone>
    {
        public Tilemap Tilemap { get; set; }
        
        public ZoneData ZoneData { get; private set; }
        
        public void Load(ZoneData zoneData = null)
        {
            ZoneData = zoneData ?? ZoneData.Generate(0, ZoneType.Beach, new Vector2Int(4, 2));
            for (var x = 0; x < ZoneData.Tiles.GetLength(0); ++x)
            for (var y = 0; y < ZoneData.Tiles.GetLength(1); ++y)
            {
                Tilemap.SetTile(
                    new Vector3Int(x, y, 0), 
                    ZoneData.Tiles[x, y]
                );
            }
        }

        protected override void Awake()
        {
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                Tilemap = rootGameObject.GetComponentInChildren<Tilemap>();
                if (Tilemap != null) break;
            }
        }
    }
}