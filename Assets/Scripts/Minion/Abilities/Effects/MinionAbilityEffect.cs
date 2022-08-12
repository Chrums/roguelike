using System;

namespace Fizz6.Roguelike.Minion.Abilities.Effects
{
    [Serializable]
    public abstract class MinionAbilityEffect
    {
        public abstract void Apply(MinionAbilityInstance abilityInstance);
    }
}