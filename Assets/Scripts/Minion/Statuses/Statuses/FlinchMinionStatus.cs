using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fizz6.Roguelike.Minion.Status
{
    [Serializable]
    public class FlinchMinionStatus : MinionStatus<FlinchMinionStatus>
    {
        [SerializeField]
        private int chance;

        protected override MinionStatusSettings Settings => new (disposeOnBattleTurnComplete: true);

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

        public bool CanUseAbilityEvent()
        {
            var random = Random.Range(0, 100);
            return random > chance;
        }

        protected override void Apply(FlinchMinionStatus status)
        {
            chance = Math.Max(chance, status.chance);
        }
    }
}
