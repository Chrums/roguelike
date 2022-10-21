using Fizz6.Roguelike.Status;

namespace Fizz6.Roguelike.Battle.Status
{
    public abstract class BattleStatus : Status<BattleStatus>
    {
        protected Battle Battle { get; private set; }

        public virtual void Initialize(Battle battle)
        {
            Battle = battle;
        }
    }

    public abstract class BattleStatus<TBattleStatus> : BattleStatus
        where TBattleStatus : BattleStatus<TBattleStatus>
    {
        public sealed override void Apply(BattleStatus status)
        {
            var battleStatus = status as TBattleStatus;
            Apply(battleStatus);
        }

        protected abstract void Apply(TBattleStatus battleStatus);
    }
}