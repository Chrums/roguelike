using System.Linq;
using Fizz6.Roguelike.Battle.Trainers;

namespace Fizz6.Roguelike.Battle.Controls.Actions
{
    public class MinionAbilityBattleAction : BattleAction
    {
        private readonly int index;
        
        public MinionAbilityBattleAction(Battle battle, BattleTrainer trainer, int index) : base(battle, trainer)
        {
            this.index = index;
        }
        
        public override void Invoke()
        {
            var minionAbilityInstance = Trainer.ActiveMinion.Abilities.Use(index);
            minionAbilityInstance.Targets = Battle.Trainers
                .Where(trainer => Trainer != trainer)
                .Select(trainer => trainer.ActiveMinion)
                .ToList();
            minionAbilityInstance = Battle.ModifyAbility(minionAbilityInstance); // TODO: Should the Battle just add modifiers to the ActiveMinion?
            minionAbilityInstance.Resolve();
        }
    }
}
