using System;
using Fizz6.Roguelike.Minion.Abilities;
using Newtonsoft.Json;
using UnityEngine;

namespace Fizz6.Roguelike.Minion.Statuses
{
    [Serializable]
    public class AccuracyMinionStatus : AbilityUserMinionStatus<AccuracyMinionStatus>
    {
        [SerializeField, JsonProperty]
        private int modifier;
        public int Modifier
        {
            get => modifier;
            set => modifier = value;
        }
        
        protected override MinionAbilityInstance OnModifyAbilityUser(MinionAbilityInstance minionAbilityInstance)
        {
            var accuracy = Mathf.RoundToInt(minionAbilityInstance.Accuracy * AccuracyMinionStatusConfig.Instance[modifier]);
            minionAbilityInstance.Accuracy = accuracy;
            return minionAbilityInstance;
        }

        protected override void Apply(AccuracyMinionStatus status)
        {
            modifier = Math.Clamp(
                modifier + status.modifier, 
                -AccuracyMinionStatusConfig.Instance.Limit, 
                AccuracyMinionStatusConfig.Instance.Limit
            );
        }
    }
}
