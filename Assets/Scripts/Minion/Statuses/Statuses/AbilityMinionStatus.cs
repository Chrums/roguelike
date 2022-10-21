using System;
using Fizz6.Roguelike.Minion.Abilities;

namespace Fizz6.Roguelike.Minion.Status
{
    [Serializable]
    public abstract class AbilityMinionStatus<TMinionStatus> : MinionStatus<TMinionStatus> where TMinionStatus : MinionStatus<TMinionStatus>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Minion.ModifyAbilityEvent += OnModifyAbility;
        }

        public override void Dispose()
        {
            Minion.ModifyAbilityEvent -= OnModifyAbility;
            base.Dispose();
        }

        protected abstract MinionAbilityInstance OnModifyAbility(MinionAbilityInstance minionAbilityInstance);
    }
}