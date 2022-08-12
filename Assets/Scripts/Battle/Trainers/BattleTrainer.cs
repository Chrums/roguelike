using System;
using System.Linq;
using Fizz6.Roguelike.Battle.Controls;

namespace Fizz6.Roguelike.Battle.Trainers
{
    public abstract class BattleTrainer
    {
        public event Action<Minion.Minion, Minion.Minion> ActiveMinionChangeEvent;
        
        public BattleControls Controls { get; }
        public Trainer.TrainerData TrainerData { get; }
        public Minion.Minion ActiveMinion { get; private set; }

        public BattleTrainer(BattleControls controls, Trainer.TrainerData trainerData)
        {
            Controls = controls;
            TrainerData = trainerData;
            ActiveMinion = trainerData.Minions.FirstOrDefault();
            
            Controls.Initialize(this);
        }

        ~BattleTrainer()
        {
            Controls.Dispose();
        }

        public void ChangeMinion(Minion.Minion minion)
        {
            if (!TrainerData.Minions.Contains(minion)) return;
            var previous = ActiveMinion;
            ActiveMinion = minion;
            ActiveMinionChangeEvent?.Invoke(previous, minion);
        }
    }
}