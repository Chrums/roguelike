using System;
using Fizz6.Roguelike.Statuses;

namespace Fizz6.Roguelike.Battle.Statuses
{
    [Serializable]
    public class BattleStatuses : Statuses<BattleStatus>, IDisposable
    {
        [NonSerialized]
        private Battle battle;

        public void Initialize(Battle battle)
        {
            this.battle = battle;
        }

        public void Dispose()
        {
            battle = null;
        }

        public override void Initialize(BattleStatus status)
        {
            base.Initialize(status);
            status.Initialize(battle);
        }
    }
}