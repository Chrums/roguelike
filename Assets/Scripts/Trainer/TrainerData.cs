using System.Collections.Generic;
using Fizz6.Roguelike.Minion;
using Newtonsoft.Json;

namespace Fizz6.Roguelike.Trainer
{
    public class TrainerData
    {
        public static TrainerData Generate(int level)
        {
            var trainerData = new TrainerData
            {
                Level = level
            };
                    
            var randomMinions = UnityEngine.Random.Range(0, 6);
            for (var index = 0; index < randomMinions; ++index)
            {
                var randomLevel = UnityEngine.Random.Range(level - 2, level + 2);
                var minion = new Minion.Minion
                {
                    Config = MinionConfig.Random,
                    Level = randomLevel
                };
                    
                trainerData.minions.Add(minion);
            }

            return trainerData;
        }

        [JsonProperty] 
        public int Level { get; private set; }
        
        [JsonProperty] 
        private List<Minion.Minion> minions = new();
        [JsonIgnore]
        public IReadOnlyList<Minion.Minion> Minions => minions;
    }
}

