using System.Threading.Tasks;
using Fizz6.Roguelike.Battle.Controls.Actions;
using Fizz6.Roguelike.Battle.Trainers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fizz6.Roguelike.Battle.Controls
{
    public class PlayerBattleController : MonoBehaviour
    {
        public static PlayerBattleController Instance { get; private set; }

        [SerializeField]
        private Button[] abilityButtons = new Button[4];

        private BattleTrainer battleTrainer;
        private TaskCompletionSource<BattleAction> battleActionTaskCompletionSource;

        private void Awake()
        {
            Instance = this;
        }

        public Task<BattleAction> GetBattleAction(BattleTrainer battleTrainer)
        {
            Configure(battleTrainer);
            
            battleActionTaskCompletionSource?.SetCanceled();
            battleActionTaskCompletionSource = new TaskCompletionSource<BattleAction>();
            battleActionTaskCompletionSource.Task
                .GetAwaiter()
                .OnCompleted(() => battleActionTaskCompletionSource = null);

            return battleActionTaskCompletionSource.Task;
        }

        private void Configure(BattleTrainer battleTrainer)
        {
            this.battleTrainer = battleTrainer;
            
            for (var index = 0; index < abilityButtons.Length; ++index)
            {
                var abilityButton = abilityButtons[index];
                var textMeshProUGUI = abilityButton.GetComponentInChildren<TextMeshProUGUI>();
                var ability = battleTrainer.ActiveMinion.Abilities[index];
                if (ability == null)
                {
                    abilityButton.interactable = false;
                    textMeshProUGUI.text = null;
                    continue;
                }
            
                textMeshProUGUI.text = ability.name;
                var value = index;
                abilityButton.onClick.AddListener(() => UseAbility(value));
            }
        }

        private void UseAbility(int index)
        {
            var battleAction = new MinionAbilityBattleAction(Battle.ActiveBattle, battleTrainer, index); // TODO: Change Battle.ActiveBattle
            battleActionTaskCompletionSource.SetResult(battleAction);
        }
    }
}