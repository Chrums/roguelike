using Fizz6.Roguelike.Battle.Trainers;

namespace Fizz6.Roguelike.Battle.Controls.Actions
{
    public abstract class BattleAction
    {
        protected Battle Battle { get; }
        protected BattleTrainer Trainer { get; }

        protected BattleAction(Battle battle, BattleTrainer trainer)
        {
            Battle = battle;
            Trainer = trainer;
        }
        
        public abstract void Invoke();
    }
}