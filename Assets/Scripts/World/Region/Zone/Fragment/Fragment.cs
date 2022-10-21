using System.Collections.Generic;
using System.Linq;
using Fizz6.Autofill;
using Fizz6.Core;
using Fizz6.Roguelike.Spawner;
using Fizz6.Roguelike.World.Region.Zone.Fragment.Tile;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fizz6.Roguelike.World.Region.Zone.Fragment
{
    public class Fragment : MonoBehaviour
    {
        private const string Path = "Fragments";
        private static Dictionary<string, Fragment> _fragments;
        public static IReadOnlyDictionary<string, Fragment> Fragments =>
            _fragments ??= Resources
                .LoadAll<Fragment>(Path)
                .ToDictionary(
                    fragment => fragment.name,
                    fragment => fragment
                );

        public static Fragment Random(IEnumerable<Fragment> except = null) => Fragments.Values.Except(except ?? new List<Fragment>()).Random();
        
        [SerializeField, Autofill(AutofillAttribute.Target.Self | AutofillAttribute.Target.Parent | AutofillAttribute.Target.Children)]
        private Tilemap tilemap;
        public Tilemap Tilemap => tilemap;

        private List<Vector3Int> _linkTiles;
        public IEnumerable<Vector3Int> LinkTiles
        {
            get
            {
                if (_linkTiles != null) return _linkTiles;
                
                var linkTiles = new List<Vector3Int>();
                
                var cellBounds = tilemap.cellBounds;
                for (var x = cellBounds.xMin; x <= cellBounds.xMax; ++x)
                for (var y = cellBounds.yMin; y <= cellBounds.yMax; ++y)
                for (var z = cellBounds.zMin; z <= cellBounds.zMax; ++z)
                {
                    var position = new Vector3Int(x, y, z);
                    var tile = tilemap.GetTile(position);
                    if (tile is LinkTile) linkTiles.Add(position);
                }

                _linkTiles = linkTiles;

                return _linkTiles;
            }
        }

        public IEnumerable<ISpawnable> Spawn(ZoneData zoneData) =>
            GetComponentsInChildren<Spawner.Spawner>()
                .Select(spawner => spawner.Spawn(zoneData))
                .Where(spawnable => spawnable != null);
    }
}
