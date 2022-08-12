using Fizz6.Actor;
using Fizz6.Autofill;
using Fizz6.Roguelike.Character.Behaviours;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fizz6.Roguelike.Character
{
    public class Character : MonoBehaviour
    {
        [SerializeField] 
        private Tilemap tilemap;
        public Tilemap Tilemap
        {
            get => tilemap;
            set => tilemap = value;
        }

        [SerializeField]
        private Actor<CharacterBehaviour> actor;

        private Vector3Int cell;
        public Vector3Int Cell
        {
            get => cell;
            set
            {
                cell = value;
                transform.position = tilemap.layoutGrid.GetCellCenterWorld(cell);
            }
        }

        private void Awake()
        {
            Cell = tilemap.layoutGrid.WorldToCell(transform.position);
            
            actor.Initialize();
            foreach (var behaviour in actor.Behaviours) behaviour.Initialize(this);
        }

        private void OnDestroy() => actor.Dispose();

        private void Update() => actor.Update();
    }
}