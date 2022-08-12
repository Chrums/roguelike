using System;
using UnityEngine;

namespace Fizz6.Roguelike.Minion.Abilities.Modifiers
{
    [Serializable]
    public class PowerMinionAbilityModifier : MinionAbilityModifier
    {
        [SerializeField]
        private int modifier;
        
        public override MinionAbilityInstance Apply(MinionAbilityInstance abilityInstance)
        {
            abilityInstance.Power += modifier;
            return abilityInstance;
        }
    }
}