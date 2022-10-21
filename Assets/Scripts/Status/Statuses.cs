using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fizz6.Roguelike.Status
{
    public interface IStatuses<in TStatusBase> 
        where TStatusBase : class, IStatus<TStatusBase>
    {
        void Dispose(TStatusBase status);
    }
    
    [Serializable]
    public abstract class Statuses<TStatusBase> : IStatuses<TStatusBase> 
        where TStatusBase : class, IStatus<TStatusBase>
    {
        [JsonProperty]
        private Dictionary<System.Type, TStatusBase> statuses = new();
        
        public bool Has<TStatus>() where TStatus : class, TStatusBase
        {
            var statusType = typeof(TStatus);
            return statuses.ContainsKey(statusType);
        }
        
        public TStatus Get<TStatus>() where TStatus : class, TStatusBase
        {
            var statusType = typeof(TStatus);
            return Get(statusType) as TStatus;
        }

        public TStatusBase Get(System.Type statusType)
        {
            if (!typeof(TStatusBase).IsAssignableFrom(statusType)) return null;
            if (statuses.TryGetValue(statusType, out var status)) return status;
            
            status = Activator.CreateInstance(statusType) as TStatusBase;
            if (status == null) return null;
            Initialize(status);
            statuses.Add(statusType, status);

            return status;
        }

        public virtual void Initialize(TStatusBase status)
        {
            status.Initialize(this);
        }

        public void Dispose(TStatusBase status)
        {
            var baseStatusType = status.GetType();
            statuses.Remove(baseStatusType);
        }
    }
}