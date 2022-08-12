using Fizz6.Core;
using UnityEngine;

namespace Fizz6.Roguelike.Minion.Statuses
{
    [CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Fizz6/Roguelike/Minion/Experience")]
    public class ExperienceConfig : SingletonScriptableObject<AccuracyMinionStatusConfig>
    {
        [SerializeField]
        private int maximumLevel = 100;
        public int MaximumLevel => maximumLevel;

        [SerializeField]
        private int maximumExperience;
        
        [SerializeField]
        private AnimationCurve curve;

        private void OnValidate()
        {
            
        }

        public int this[int level] => 
            Mathf.FloorToInt(
                curve.Evaluate((float)level / maximumLevel) * maximumExperience
            );

        // public int Reward(Minion minion)
        // {
        //     var requirement = this[minion.Level];
        // }
    }
}