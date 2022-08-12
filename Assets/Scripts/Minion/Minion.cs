using System;
using Fizz6.Core;
using Fizz6.Roguelike.Minion.Abilities;
using Fizz6.Roguelike.Minion.Items;
using Fizz6.Roguelike.Minion.Statistics;
using Fizz6.Roguelike.Minion.Statuses;
using Newtonsoft.Json;

namespace Fizz6.Roguelike.Minion
{
    public class Minion
    {
        [JsonProperty]
        public MinionConfig Config { get; set; }
        
        [JsonProperty] 
        public int Level { get; set; }

        [JsonProperty]
        public int Experience { get; set; }

        [JsonProperty] 
        public MinionItem Item { get; set; }
        
        [JsonProperty] 
        public MinionStatistics Statistics { get; } = new();

        [JsonProperty] 
        public MinionStatuses Statuses { get; } = new();
        
        [JsonProperty]
        public MinionAbilities Abilities { get; } = new();

        [JsonProperty]
        public int Life { get; set; }
        
        [JsonIgnore]
        public bool IsFainted => Life == 0;

        [JsonIgnore]
        public bool CanUseAbility => CanUseAbilityEvent.Aggregate();

        public event Action<int> DamageEvent;
        public event Action FaintEvent;
        
        public event Func<bool> CanUseAbilityEvent;
        public event Func<MinionAbilityInstance, MinionAbilityInstance> ModifyAbilityEvent;

        public MinionAbilityInstance ModifyAbility(MinionAbilityInstance minionAbilityInstance) =>
            ModifyAbilityEvent.Aggregate(minionAbilityInstance);

        public void Initialize()
        {
            Statuses.Initialize(this);
            Abilities.Initialize(this);
        }

        public void Dispose()
        {
            Statuses.Dispose();
            Abilities.Dispose();
        }

        public void Damage(int value)
        {
            if (IsFainted) return;
            value = Math.Min(Life, value);
            Life -= value;
            DamageEvent?.Invoke(value);
            if (IsFainted) FaintEvent?.Invoke();
        }
    }
}