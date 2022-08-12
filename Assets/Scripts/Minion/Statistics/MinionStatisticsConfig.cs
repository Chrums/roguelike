using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fizz6.Roguelike.Minion.Statistics
{
    [CreateAssetMenu(fileName = "Statistics", menuName = "Fizz6/Roguelike/Minion/Statistics")]
    public class MinionStatisticsConfig : ScriptableObject
    {
        [SerializeField] 
        private MinionStatisticConfig vitality;

        [SerializeField]
        private MinionStatisticConfig agility;

        [SerializeField]
        private MinionStatisticConfig strength;

        [SerializeField]
        private MinionStatisticConfig toughness;

        [SerializeField]
        private MinionStatisticConfig wisdom;

        [SerializeField]
        private MinionStatisticConfig willpower;
        
        private Dictionary<MinionStatisticType, MinionStatisticConfig> statistics = new ();
        public IReadOnlyDictionary<MinionStatisticType, MinionStatisticConfig> Statistics => statistics;

        private void Awake()
        {
            statistics[MinionStatisticType.Vitality] = vitality;
            statistics[MinionStatisticType.Agility] = agility;
            statistics[MinionStatisticType.Strength] = strength;
            statistics[MinionStatisticType.Toughness] = toughness;
            statistics[MinionStatisticType.Wisdom] = wisdom;
            statistics[MinionStatisticType.Willpower] = willpower;
        }

        private void OnDestroy()
        {
            statistics.Clear();
        }
    }
}