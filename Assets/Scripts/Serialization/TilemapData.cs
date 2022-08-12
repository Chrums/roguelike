using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;
// using Tile = Fizz6.Roguelike.World.Map.Tiles.Tile;

namespace Fizz6.Roguelike.Serialization
{
    // public class TilemapData : Data<Tilemap>
    // {
    //     public class Item
    //     {
    //         [JsonProperty]
    //         public Vector3Int Cell { get; set; }
    //         
    //         [JsonProperty]
    //         public Tile Tile { get; set; }
    //     }
    //
    //     [JsonProperty]
    //     private readonly List<Item> items = new();
    //     [JsonIgnore]
    //     public IReadOnlyList<Item> Items => items;
    //
    //     public void From(Tilemap tilemap)
    //     {
    //         foreach (var cell in tilemap.cellBounds.allPositionsWithin)
    //         {
    //             var tile = tilemap.GetTile<Tile>(cell);
    //             var item = new Item
    //             {
    //                 Cell = cell,
    //                 Tile = tile
    //             };
    //             
    //             items.Add(item);
    //         }
    //     }
    //
    //     public void To(ref Tilemap tilemap)
    //     {
    //         foreach (var item in items)
    //         {
    //             tilemap.SetTile(item.Cell, item.Tile);
    //         }
    //     }
    // }
}