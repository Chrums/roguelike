using Fizz6.Core;
using UnityEngine;

namespace Fizz6.Roguelike.Minion.Statuses
{
    [CreateAssetMenu(fileName = "AccuracyMinionStatusConfig", menuName = "Fizz6/Roguelike/Minion/Statuses/Accuracy")]
    public class AccuracyMinionStatusConfig : SingletonScriptableObject<AccuracyMinionStatusConfig>
    {
        [SerializeField]
        private int limit;
        public int Limit => limit;
        
        [SerializeField]
        private float[] multipliers;

        public float this[int modifier] => multipliers[modifier + limit];

        private void OnValidate()
        {
            var size = limit * 2 + 1;
            if (multipliers.Length == size) return;
            multipliers = new float[size];
        }
    }
}