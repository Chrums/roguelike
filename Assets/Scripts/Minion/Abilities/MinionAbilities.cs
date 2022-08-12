using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fizz6.Roguelike.Minion.Abilities
{
    public class MinionAbilities
    {
        [JsonProperty]
        private readonly Dictionary<int, MinionAbilityConfig> abilities = new();

        [JsonIgnore]
        private Minion minion;
        
        public MinionAbilityConfig this[int index]
        {
            get => abilities[index];
            set => abilities[index] = value;
        }
        
        public void Initialize(Minion minion)
        {
            this.minion = minion;
        }

        public void Dispose()
        {
            minion = null;
        }

        public bool Has(int index) => abilities.ContainsKey(index);

        public MinionAbilityInstance Use(int index)
        {
            var abilityInstance = abilities[index].Invoke();
            abilityInstance.User = minion;
            return abilityInstance;
        }
    }
}