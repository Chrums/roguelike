using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Roguelike.Minion.Abilities;
using Newtonsoft.Json;
using UnityEngine;

namespace Fizz6.Roguelike.Minion
{
    [CreateAssetMenu(fileName = "Minion", menuName = "Fizz6/Roguelike/Minion/Minion")]
    public class MinionConfig : ScriptableObject
    {
        private const string Path = "Minions";
        private static Dictionary<string, MinionConfig> _minionConfigs;
        private static IReadOnlyDictionary<string, MinionConfig> MinionConfigs =>
            _minionConfigs ??= Resources
                .LoadAll<MinionConfig>(Path)
                .ToDictionary(config => config.name, config => config);

        public static MinionConfig Random
        {
            get
            {
                var index = UnityEngine.Random.Range(0, MinionConfigs.Count);
                return MinionConfigs.ElementAtOrDefault(index).Value;
            }
        }
        
        [Serializable]
        public struct AbilityLevel
        {
            [SerializeField] 
            private int level;
            public int Level => level;
            
            [SerializeField] 
            private MinionAbilityConfig minionAbilityConfig;
            public MinionAbilityConfig MinionAbilityConfig => minionAbilityConfig;
        }
        
        [SerializeField] 
        private List<Type> types;
        public IReadOnlyList<Type> Types => types;
        
        [SerializeField]
        private Statistics.MinionStatisticsConfig minionStatisticsConfig;
        public Statistics.MinionStatisticsConfig MinionStatisticsConfig => minionStatisticsConfig;

        [SerializeField] 
        private List<AbilityLevel> abilities;
        public List<AbilityLevel> Abilities => abilities;

        public class Converter : JsonConverter<MinionConfig>
        {
            public override void WriteJson(JsonWriter writer, MinionConfig value, JsonSerializer serializer)
            {
                writer.WriteValue(value.name);
            }

            public override MinionConfig ReadJson(JsonReader reader, System.Type objectType, MinionConfig existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var name = reader.Value?.ToString();
                return name != null 
                    ? MinionConfigs[name]
                    : existingValue;
            }
        }
    }
}