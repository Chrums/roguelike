using System;
using UnityEngine;

namespace Fizz6.Roguelike.Input
{
    [RequireComponent(typeof(Collider2D))]
    public class MouseEventListener : MonoBehaviour
    {
        public event Action MouseEnterEvent;
        public event Action MouseExitEvent;
        public event Action MouseOverEvent;
        public event Action MouseDragEvent;
        public event Action MouseDownEvent;
        public event Action MouseUpEvent;

        private void OnMouseEnter() =>
            MouseEnterEvent?.Invoke();

        private void OnMouseExit() =>
            MouseExitEvent?.Invoke();

        private void OnMouseOver() =>
            MouseOverEvent?.Invoke();

        private void OnMouseDrag() =>
            MouseDragEvent?.Invoke();

        private void OnMouseDown() =>
            MouseDownEvent?.Invoke();

        private void OnMouseUp() =>
            MouseUpEvent?.Invoke();
    }
}