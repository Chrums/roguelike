using UnityEngine;

namespace Fizz6.Roguelike.Trainer
{
    public class Trainer : MonoBehaviour
    {
        public Actor.Actor<TrainerBehaviour> Actor { get; private set; } = new();

        public void Initialize(TrainerData trainerData)
        {
            
        }
    }
}