using System;
using Fizz6.Roguelike.Status;

namespace Fizz6.Roguelike.Minion.Status
{
    [Serializable]
    public abstract class MinionStatus : Status<MinionStatus>
    {
        protected class MinionStatusSettings
        {
            public static MinionStatusSettings Default => new();

            public bool DisposeOnMinionDeath { get; }
            public bool DisposeOnBattleComplete { get; }
            public bool DisposeOnBattleTurnComplete { get; }

            public MinionStatusSettings(
                bool disposeOnMinionDeath = true, 
                bool disposeOnBattleComplete = true,
                bool disposeOnBattleTurnComplete = false
            )
            {
                DisposeOnMinionDeath = disposeOnMinionDeath;
                DisposeOnBattleComplete = disposeOnBattleComplete;
                DisposeOnBattleTurnComplete = disposeOnBattleTurnComplete;
            }
        }
        
        protected Minion Minion { get; private set; }
        
        protected virtual MinionStatusSettings Settings => MinionStatusSettings.Default;

        public virtual void Initialize(Minion minion)
        {
            Minion = minion;
            
            Minion.FaintEvent += OnMinionFaint;
            Battle.Battle.EndEvent += OnBattleEnd;

            if (Battle.Battle.ActiveBattle != null)
            {
                Battle.Battle.ActiveBattle.TurnEndEvent += OnBattleTurnEnd;
            }
            
            Initialize();
        }
        
        protected virtual void Initialize()
        {}

        public override void Dispose()
        {
            Minion.FaintEvent -= OnMinionFaint;
            Battle.Battle.EndEvent -= OnBattleEnd;
            
            if (Battle.Battle.ActiveBattle != null)
            {
                Battle.Battle.ActiveBattle.TurnEndEvent -= OnBattleTurnEnd;
            }
            
            Minion = null;
            
            base.Dispose();
        }
        
        protected virtual void OnMinionFaint()
        {
            if (Settings.DisposeOnMinionDeath) Dispose();
        }

        protected virtual void OnBattleEnd(Battle.Battle battle)
        {
            if (Settings.DisposeOnBattleComplete) Dispose();
        }

        protected virtual void OnBattleTurnEnd()
        {
            if (Settings.DisposeOnBattleTurnComplete) Dispose();
        }
    }

    public abstract class MinionStatus<TMinionStatus> : MinionStatus
        where TMinionStatus : MinionStatus<TMinionStatus>
    {
        public sealed override void Apply(MinionStatus status)
        {
            var minionStatus = status as TMinionStatus;
            Apply(minionStatus);
        }

        protected abstract void Apply(TMinionStatus minionStatus);
    }
}