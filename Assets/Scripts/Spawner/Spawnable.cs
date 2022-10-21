using Newtonsoft.Json;
using UnityEngine;

namespace Fizz6.Roguelike.Spawner
{
    public interface ISpawnable
    {
        public Matrix4x4 Transform { get; }
        public void Initialize(GameObject gameObject);
    }
    
    public abstract class Spawnable<TComponent> : ISpawnable where TComponent : Component
    {
        [JsonProperty]
        public Matrix4x4 Transform { get; set; }
        
        [JsonIgnore]
        public GameObject GameObject { get; private set; }
        
        [JsonIgnore]
        public TComponent Component { get; private set; }
        
        public void Initialize(GameObject gameObject)
        {
            GameObject = gameObject;
            Component = gameObject.AddComponent<TComponent>();
        }
        
        protected virtual void Initialize() {}
    }
}