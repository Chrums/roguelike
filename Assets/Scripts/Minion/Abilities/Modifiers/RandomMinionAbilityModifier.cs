using System;
using System.Collections.Generic;
using Fizz6.SerializeImplementation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fizz6.Roguelike.Minion.Abilities.Modifiers
{
    [Serializable]
    public class RandomMinionAbilityModifier : MinionAbilityModifier
    {
        [SerializeReference, SerializeImplementation]
        private List<MinionAbilityModifier> modifiers;
        
        public override MinionAbilityInstance Apply(MinionAbilityInstance abilityInstance)
        {
            var index = Random.Range(0, modifiers.Count);
            abilityInstance = modifiers[index].Apply(abilityInstance);
            return abilityInstance;
        }
    }
}