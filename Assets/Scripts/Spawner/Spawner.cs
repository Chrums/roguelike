using System;
using Fizz6.Roguelike.World.Region.Zone;
using Fizz6.SerializeImplementation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fizz6.Roguelike.Spawner
{
    public class Spawner : MonoBehaviour
    {
        [Serializable]
        public abstract class Implementation
        {
            public abstract ISpawnable Spawn(ZoneData zoneData);
        }

        [SerializeReference, SerializeImplementation]
        private Implementation implementation;
        
        [SerializeField, Range(0.0f, 1.0f)] 
        private float chance = 0.5f;

        public ISpawnable Spawn(ZoneData zoneData)
        {
            var random = Random.Range(0.0f, 1.0f);
            return random <= chance
                ? implementation.Spawn(zoneData)
                : null;
        }
    }
}