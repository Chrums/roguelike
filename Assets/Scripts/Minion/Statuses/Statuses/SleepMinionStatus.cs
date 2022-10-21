using System;
using Random = UnityEngine.Random;

namespace Fizz6.Roguelike.Minion.Status
{
    [Serializable]
    public class SleepMinionStatus : MinionStatus<SleepMinionStatus>
    {
        private int turns;
        
        protected override void Initialize()
        {
            base.Initialize();
            Minion.CanUseAbilityEvent += CanUseAbilityEvent;
        }

        public override void Dispose()
        {
            Minion.CanUseAbilityEvent -= CanUseAbilityEvent;
            base.Dispose();
        }

        public bool CanUseAbilityEvent() => false;

        protected override void Apply(SleepMinionStatus status)
        {
            if (turns > 0) return;
            turns = Random.Range(
                SleepMinionStatusConfig.Instance.MinimumTurns, 
                SleepMinionStatusConfig.Instance.MaximumTurns
            );
        }

        protected override void OnBattleTurnEnd()
        {
            base.OnBattleTurnEnd();
            if (--turns > 0) return;
            Dispose();
        }
    }
}