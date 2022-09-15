using Fizz6.Autofill;
using UnityEngine;

namespace Fizz6.Roguelike.Camera
{
    public class DragCameraController : MonoBehaviour
    {
        [SerializeField, Autofill]
        private UnityEngine.Camera camera;
        
        [SerializeField] 
        private float positionMultiplier = 0.1f;
        
        [SerializeField] 
        private float zoomMultiplier = 2.0f;

        [SerializeField] 
        private bool invert = false;
        
        private Vector3 initialPosition;
        private Vector3 initialMousePosition;
        
        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                initialPosition = transform.position;
                initialMousePosition = UnityEngine.Input.mousePosition;
            }

            if (UnityEngine.Input.GetMouseButton(0))
            {
                var deltaPosition = (UnityEngine.Input.mousePosition - initialMousePosition) * positionMultiplier * (invert ? -1.0f : 1.0f);
                transform.position = initialPosition + deltaPosition;
            }

            var zoom = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            if (zoom != 0.0f)
            {
                camera.orthographicSize -= zoom * zoomMultiplier;
            }
        }
    }
}