using Fizz6.Core;
using UnityEngine;

namespace Fizz6.Roguelike.Minion.Statuses
{
    [CreateAssetMenu(fileName = "SleepMinionStatusConfig", menuName = "Fizz6/Roguelike/Minion/Statuses/Sleep")]
    public class SleepMinionStatusConfig : SingletonScriptableObject<SleepMinionStatusConfig>
    {
        [SerializeField] 
        private int minimumTurns;
        public int MinimumTurns => minimumTurns;

        [SerializeField] 
        private int maximumTurns;
        public int MaximumTurns => maximumTurns;
    }
}