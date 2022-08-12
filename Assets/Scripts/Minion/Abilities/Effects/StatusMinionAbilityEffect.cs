using System;
using Fizz6.Roguelike.Minion.Statuses;
using Fizz6.SerializeImplementation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fizz6.Roguelike.Minion.Abilities.Effects
{
    [Serializable]
    public class StatusMinionAbilityEffect : MinionAbilityEffect
    {
        [SerializeField, Range(0, 100)] 
        private int chance = 100;
        
        [SerializeReference, SerializeImplementation] 
        private MinionStatus status;
        
        public override void Apply(MinionAbilityInstance abilityInstance)
        {
            var random = Random.Range(0, 100);
            if (random >= chance) return;
            
            var statusType = status.GetType();
            foreach (var target in abilityInstance.Targets)
            {
                var currentStatus = target.Statuses.Get(statusType);
                currentStatus.Apply(status);
            }
        }
    }
}
