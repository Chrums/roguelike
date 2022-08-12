using UnityEngine;

namespace Fizz6.Roguelike.Camera
{
    public class TargetCameraController : MonoBehaviour
    {
        [SerializeField] 
        private GameObject target;
        public GameObject Target
        {
            get => target;
            set => target = value;
        }
        
        public void Update()
        {
            if (!Target) return;
            transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
        }
    }
}