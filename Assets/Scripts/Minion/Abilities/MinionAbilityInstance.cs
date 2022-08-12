using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fizz6.Roguelike.Minion.Abilities
{
    public struct MinionAbilityInstance
    {
        private static Dictionary<Type, Dictionary<Type, float>> TypeMultipliers = new(); 
        
        public MinionAbilityConfig Config { get; }
        
        public Minion User { get; set; }
        public List<Minion> Targets { get; set; }
        
        public Type Type { get; set; }
        public int Power { get; set; }
        public int Accuracy { get; set; }
        public float Critical { get; set; }

        public event Action<MinionAbilityInstance> MissEvent;
        public event Action<MinionAbilityInstance> HitEvent;
        public event Action<MinionAbilityInstance> CriticalEvent;

        public MinionAbilityInstance(MinionAbilityConfig config)
        {
            User = null;
            Targets = null;
            Config = config;
            Type = config.Type;
            Power = config.Power;
            Accuracy = config.Accuracy;
            Critical = 1.0f / 24.0f;
            MissEvent = default;
            HitEvent = default;
            CriticalEvent = default;
        }

        private MinionAbilityInstance(MinionAbilityInstance minionAbilityInstance, Minion target)
        {
            Config = minionAbilityInstance.Config;
            User = minionAbilityInstance.User;
            Targets = new List<Minion> { target };
            Type = minionAbilityInstance.Type;
            Power = minionAbilityInstance.Power;
            Accuracy = minionAbilityInstance.Accuracy;
            Critical = minionAbilityInstance.Critical;
            MissEvent = minionAbilityInstance.MissEvent;
            HitEvent = minionAbilityInstance.HitEvent;
            CriticalEvent = minionAbilityInstance.CriticalEvent;
        }

        public void Resolve()
        {
            var minionAbilityInstance = User.ModifyAbility(this);
            var typeMultipliers = TypeMultipliers[minionAbilityInstance.Type];
            foreach (var target in minionAbilityInstance.Targets)
            {
                var targetMinionAbilityInstance = new MinionAbilityInstance(this, target);
                targetMinionAbilityInstance = target.ModifyAbility(targetMinionAbilityInstance);
                
                var hitRandom = Random.Range(0, 100);
                var isHit = hitRandom < targetMinionAbilityInstance.Accuracy;
                if (isHit)
                {
                    var typeMultiplier = target.Config.Types
                        .Aggregate(1.0f, (current, type) => current * typeMultipliers[type]);

                    var damage = Mathf.RoundToInt(targetMinionAbilityInstance.Power * typeMultiplier);

                    var criticalRandom = Random.Range(0.0f, 1.0f);
                    var isCritical = criticalRandom < targetMinionAbilityInstance.Critical;
                    if (isCritical)
                    {
                        damage = Mathf.RoundToInt(damage * 2.0f);
                        CriticalEvent?.Invoke(targetMinionAbilityInstance);
                    }
                    
                    target.Damage(damage);
                    
                    HitEvent?.Invoke(targetMinionAbilityInstance);
                }
                else
                {
                    MissEvent?.Invoke(targetMinionAbilityInstance);
                }
            }
        }
    }
}