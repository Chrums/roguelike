using System;
using System.Threading.Tasks;
using Fizz6.Roguelike.Battle.Controls.Actions;
using Fizz6.Roguelike.Battle.Trainers;

namespace Fizz6.Roguelike.Battle.Controls
{
    public abstract class BattleControls : IDisposable
    {
        protected BattleTrainer BattleTrainer { get; private set; }

        public void Initialize(BattleTrainer battleTrainer)
        {
            BattleTrainer = battleTrainer;
        }

        public void Dispose()
        {
            BattleTrainer = null;
        }
        
        public abstract Task<BattleAction> GetBattleAction();
    }
}   