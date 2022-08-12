using System.Threading.Tasks;
using Fizz6.Roguelike.Battle.Controls.Actions;

namespace Fizz6.Roguelike.Battle.Controls
{
    public class PlayerBattleControls : BattleControls
    {
        public override Task<BattleAction> GetBattleAction() =>
            PlayerBattleController.Instance.GetBattleAction(BattleTrainer);
    }
}
