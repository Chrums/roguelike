using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fizz6.Core;
using Fizz6.Roguelike.Battle.Statuses;
using Fizz6.Roguelike.Battle.Trainers;
using Fizz6.Roguelike.Minion.Abilities;
using UnityEngine;

namespace Fizz6.Roguelike.Battle
{
    [Serializable]
    public class Battle
    {
        public static event Action<Battle> BeginEvent;
        public static event Action<Battle> EndEvent;

        public static Battle ActiveBattle { get; private set; }

        public event Func<MinionAbilityInstance, MinionAbilityInstance> ModifyAbilityEvent;
        public event Action TurnBeginEvent;
        public event Action TurnEndEvent;
        public event Action CompleteEvent;

        public static Battle Instantiate(List<BattleTrainer> trainers)
        {
            var battle = new Battle(trainers);
            BeginEvent?.Invoke(battle);
            battle.CompleteEvent += () => EndEvent?.Invoke(battle);
            return battle;
        }
        
        private List<BattleTrainer> trainers;
        public IReadOnlyList<BattleTrainer> Trainers => trainers;

        [SerializeField]
        private BattleStatuses statuses;
        public BattleStatuses Statuses => statuses;

        public Battle(List<BattleTrainer> trainers)
        {
            this.trainers = trainers;
        }

        private async void Run()
        {
            TurnBeginEvent?.Invoke();
            
            var tasks = Trainers
                .Select(trainer => trainer.Controls.GetBattleAction())
                .ToArray();
            var battleActions = await Task.WhenAll(tasks);
            foreach (var battleAction in battleActions)
                battleAction.Invoke();
            
            TurnEndEvent?.Invoke();
        }

        public MinionAbilityInstance ModifyAbility(MinionAbilityInstance minionAbilityInstance) =>
            ModifyAbilityEvent.Aggregate(minionAbilityInstance);
    }
}