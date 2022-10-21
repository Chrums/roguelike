using System;
using Fizz6.Roguelike.Status;
using Newtonsoft.Json;

namespace Fizz6.Roguelike.Minion.Status
{
    [Serializable]
    public class MinionStatuses : Statuses<MinionStatus>, IDisposable
    {
        [JsonIgnore]
        private Minion minion;
        
        public void Initialize(Minion minion)
        {
            this.minion = minion;
        }

        public void Dispose()
        {
            minion = null;
        }

        public override void Initialize(MinionStatus minionStatus)
        {
            base.Initialize(minionStatus);
            minionStatus.Initialize(minion);
        }
    }
}