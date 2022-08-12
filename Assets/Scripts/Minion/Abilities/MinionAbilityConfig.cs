using System.Collections.Generic;
using System.Linq;
using Fizz6.Roguelike.Minion.Abilities.Effects;
using Fizz6.Roguelike.Minion.Abilities.Modifiers;
using Fizz6.SerializeImplementation;
using Newtonsoft.Json;
using UnityEngine;

namespace Fizz6.Roguelike.Minion.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Fizz6/Roguelike/Minion/Ability")]
    public class MinionAbilityConfig : ScriptableObject
    {
        private const string Path = "Abilities";
        private static Dictionary<string, MinionAbilityConfig> _minionAbilityConfigs;
        private static IReadOnlyDictionary<string, MinionAbilityConfig> MinionAbilityConfigs =>
            _minionAbilityConfigs ??= Resources
                .LoadAll<MinionAbilityConfig>(Path)
                .ToDictionary(config => config.name, config => config);
        
        [SerializeField] 
        private MinionAbilityType abilityType;
        public MinionAbilityType AbilityType => abilityType;

        [SerializeField]
        private Type type;
        public Type Type => type;
        
        [SerializeField]
        private int power;
        public int Power => power;

        [SerializeField, Range(0, 100)]
        private int accuracy = 100;
        public int Accuracy => accuracy;

        [SerializeField] 
        private int charges = 20;
        public int Charges => charges;

        [SerializeReference, SerializeImplementation]
        private List<MinionAbilityModifier> modifiers;

        [SerializeReference, SerializeImplementation]
        private List<MinionAbilityEffect> missEffects;

        [SerializeReference, SerializeImplementation]
        private List<MinionAbilityEffect> hitEffects;

        [SerializeReference, SerializeImplementation]
        private List<MinionAbilityEffect> criticalEffects;

        public MinionAbilityInstance Invoke()
        {
            var abilityInstance = new MinionAbilityInstance(this);

            abilityInstance.MissEvent += OnMiss;
            abilityInstance.HitEvent += OnHit;
            abilityInstance.CriticalEvent += OnCritical;

            return modifiers.Aggregate(
                abilityInstance, 
                (current, modifier) => modifier.Apply(current)
            );
        }

        private void OnMiss(MinionAbilityInstance abilityInstance)
        {
            foreach (var effect in missEffects) 
                effect.Apply(abilityInstance);
        }

        private void OnHit(MinionAbilityInstance abilityInstance)
        {
            foreach (var effect in hitEffects) 
                effect.Apply(abilityInstance);
        }

        private void OnCritical(MinionAbilityInstance abilityInstance)
        {
            foreach (var effect in criticalEffects) 
                effect.Apply(abilityInstance);
        }
        
        public class Converter : JsonConverter<MinionAbilityConfig>
        {
            public override void WriteJson(JsonWriter writer, MinionAbilityConfig value, JsonSerializer serializer)
            {
                writer.WriteValue(value.name);
            }

            public override MinionAbilityConfig ReadJson(JsonReader reader, System.Type objectType, MinionAbilityConfig existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var name = reader.Value?.ToString();
                return name != null 
                    ? MinionAbilityConfigs[name]
                    : existingValue;
            }
        }
    }
}
