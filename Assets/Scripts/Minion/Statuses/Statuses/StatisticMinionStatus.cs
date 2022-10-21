using System;
using Fizz6.Roguelike.Minion.Statistics;

namespace Fizz6.Roguelike.Minion.Status
{
    [Serializable]
    public abstract class StatisticMinionStatus<TMinionStatus> : MinionStatus<TMinionStatus> where TMinionStatus : MinionStatus<TMinionStatus>
    {
        private int modifier;
        
        protected abstract MinionStatisticType StatisticType { get; }

        public void Apply()
        {
            // var minionStatistic = Minion.
        }
    }
}