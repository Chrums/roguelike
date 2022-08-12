using System;

namespace Fizz6.Roguelike.Minion.Abilities.Modifiers
{
    [Serializable]
    public abstract class MinionAbilityModifier
    {
        public abstract MinionAbilityInstance Apply(MinionAbilityInstance abilityInstance);
    }
}