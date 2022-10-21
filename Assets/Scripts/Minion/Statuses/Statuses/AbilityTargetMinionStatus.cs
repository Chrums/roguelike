using System;
using Fizz6.Roguelike.Minion.Abilities;

namespace Fizz6.Roguelike.Minion.Status
{
    [Serializable]
    public abstract class AbilityTargetMinionStatus<TMinionStatus> : AbilityMinionStatus<TMinionStatus> where TMinionStatus : MinionStatus<TMinionStatus>
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

        protected override MinionAbilityInstance OnModifyAbility(MinionAbilityInstance minionAbilityInstance) =>
            minionAbilityInstance.Targets.Contains(Minion)
                ? OnModifyAbilityTarget(minionAbilityInstance)
                : minionAbilityInstance;

        protected abstract MinionAbilityInstance OnModifyAbilityTarget(MinionAbilityInstance minionAbilityInstance);
    }
}