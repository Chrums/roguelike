using System;
using UnityEngine;

namespace Fizz6.Roguelike.Camera
{
    public class AxisCameraController : MonoBehaviour
    {
        [SerializeField]
        private float minimumSpeed = 0.2f;
        [SerializeField]
        private float maximumSpeed = 1.0f;
        [SerializeField]
        private AnimationCurve speedCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] 
        private float speedTransitionTime = 1.0f;

        [SerializeField, Range(0.0f, 0.2f)]
        private float buffer = 0.1f;

        private float initialTime = 0.0f;
        private float currentSpeed = 0.0f;
        
        private void Update()
        {
            var xAxis = UnityEngine.Input.GetAxis("Horizontal");
            var xMouse = UnityEngine.Input.mousePosition.x < buffer * Screen.width
                ? -1.0f 
                : UnityEngine.Input.mousePosition.x > UnityEngine.Screen.width - buffer * Screen.width
                    ? 1.0f 
                    : 0.0f;
            var x = xAxis + xMouse;
            var yAxis = UnityEngine.Input.GetAxis("Vertical");
            var yMouse = UnityEngine.Input.mousePosition.y < buffer * Screen.height
                ? -1.0f 
                : UnityEngine.Input.mousePosition.y > UnityEngine.Screen.height - buffer * Screen.height
                    ? 1.0f 
                    : 0.0f;
            var y = yAxis + yMouse;
            if (x != 0.0f || y != 0.0f)
            {
                if (currentSpeed == 0.0f)
                {
                    initialTime = Time.time;
                    currentSpeed = minimumSpeed;
                }
                else
                {
                    var time = Mathf.Min(Time.time - initialTime, speedTransitionTime) / speedTransitionTime;
                    currentSpeed = speedCurve.Evaluate(time) * (maximumSpeed - minimumSpeed) + minimumSpeed;
                }

                transform.position = new Vector3(
                    transform.position.x + x * currentSpeed,
                    transform.position.y + y * currentSpeed,
                    transform.position.z
                );
            }
            else currentSpeed = 0.0f;
        }
    }
}