using UnityEngine;

namespace Fizz6.Roguelike
{
    public class Boot : MonoBehaviour
    {
        private async void Awake() => await Core.Load();
        private async void OnApplicationQuit() => await Core.Save();
    }
}