using System;
// using Fizz6.Roguelike.World.Map.Tiles;
// using Fizz6.Roguelike.World.Tiles;
using UnityEngine;
using Tile = UnityEngine.Tilemaps.Tile;

namespace Fizz6.Roguelike.Character.Behaviours
{
    [Serializable]
    public class MovementCharacterBehaviour : CharacterBehaviour
    {
        [SerializeField] 
        private float duration;
        
        [NonSerialized]
        private Vector2Int direction;

        [NonSerialized] 
        private Vector3Int targetCell;
        
        [NonSerialized]
        private Vector3 initialCharacterPosition;
        
        [NonSerialized]
        private Vector3 targetCharacterPosition;
        
        [NonSerialized]
        private float initialTime;
        
        public override bool Query()
        {
            direction = Vector2Int.zero;
            var horizontal = UnityEngine.Input.GetAxis("Horizontal");
            var x = horizontal == 0.0f ? 0 : horizontal > 0 ? 1 : -1;
            if (direction == Vector2Int.zero && x != 0) direction = new Vector2Int(x, 0);
            var vertical = UnityEngine.Input.GetAxis("Vertical");
            var y = vertical == 0.0f ? 0 : vertical > 0 ? 1 : -1;
            if (direction == Vector2Int.zero && y != 0) direction = new Vector2Int(0, y);
            
            if (direction == Vector2Int.zero) return false;
            
            targetCell = new Vector3Int(Character.Cell.x + direction.x, Character.Cell.y + direction.y, Character.Cell.z);
            // targetTile = Character.Tilemap.GetTile<Fizz6.Roguelike.World.Map.Tiles.Tile>(targetCell);

            return true; //  targetTile.colliderType == Tile.ColliderType.None && targetTile.CanEnter(targetCell, Character);
        }

        public override void Activate()
        {
            base.Activate();
            initialCharacterPosition = Character.transform.position;
            targetCharacterPosition = Character.Tilemap.layoutGrid.GetCellCenterWorld(targetCell);
            initialTime = Time.time;
        }

        public override void Deactivate()
        {
            var position = Character.transform.position;
            direction = Vector2Int.zero;
            initialCharacterPosition = position;
            targetCharacterPosition = position;
            initialTime = 0.0f;
            base.Deactivate();
        }

        public override void Update()
        {
            var time = Mathf.Min(1.0f, (Time.time - initialTime) / duration);
            Character.transform.position = Vector3.Lerp(initialCharacterPosition, targetCharacterPosition, time);
            if (time < 1.0f) return;
            Character.Cell = Character.Tilemap.layoutGrid.WorldToCell(Character.transform.position);
            // if (targetTile is IceTile)
            // {
            //     targetCell = new Vector3Int(targetCell.x + direction.x, targetCell.y + direction.y, targetCell.z);
            //     targetTile = Character.Tilemap.GetTile<Fizz6.Roguelike.World.Map.Tiles.Tile>(targetCell);
            //     if (targetTile.colliderType == Tile.ColliderType.None && targetTile.CanEnter(targetCell, Character))
            //     {
            //         initialCharacterPosition = Character.transform.position;
            //         targetCharacterPosition = Character.Tilemap.layoutGrid.GetCellCenterWorld(targetCell);
            //         initialTime = Time.time;
            //         return;
            //     }
            // }
            
            Yield();
        }
    }
}