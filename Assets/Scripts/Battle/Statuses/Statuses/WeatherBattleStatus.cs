using System;
using Fizz6.Roguelike.Minion.Abilities;

namespace Fizz6.Roguelike.Battle.Statuses
{
    [Serializable]
    public class WeatherBattleStatus : BattleStatus<WeatherBattleStatus>
    {
        public override void Initialize(Battle battle)
        {
            base.Initialize(battle);
            Battle.ModifyAbilityEvent += OnBattleModifyAbility;
        }

        public override void Dispose()
        {
            Battle.ModifyAbilityEvent -= OnBattleModifyAbility;
            base.Dispose();
        }

        protected override void Apply(WeatherBattleStatus status)
        {}

        private MinionAbilityInstance OnBattleModifyAbility(MinionAbilityInstance abilityInstance)
        {
            return abilityInstance;
        }
    }
}