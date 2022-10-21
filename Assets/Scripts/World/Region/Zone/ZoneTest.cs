using System.Collections;
using System.Collections.Generic;
using Fizz6.Autofill;
using Fizz6.Roguelike.World.Region.Zone;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneTest : MonoBehaviour
{
    [SerializeField, Autofill]
    private Tilemap tilemap;

    private void Awake()
    {
        var zoneData = ZoneData.Generate(0, ZoneType.Town, new Vector2Int(4, 4));
        for (var x = 0; x < zoneData.Tiles.GetLength(0); ++x)
        for (var y = 0; y < zoneData.Tiles.GetLength(1); ++y)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), zoneData.Tiles[x, y]);
        }
    }
}
