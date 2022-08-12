using System;

namespace Fizz6.Roguelike.Statuses
{
    public interface IStatus<TStatusBase> : IDisposable 
        where TStatusBase : class, IStatus<TStatusBase>
    {
        void Initialize(IStatuses<TStatusBase> statuses);
        void Apply(TStatusBase status);
    }

    [Serializable]
    public abstract class Status<TStatusBase> : IStatus<TStatusBase>
        where TStatusBase : Status<TStatusBase>
    {
        private IStatuses<TStatusBase> statuses;

        public void Initialize(IStatuses<TStatusBase> statuses)
        {
            this.statuses = statuses;
        }

        public virtual void Dispose()
        {
            statuses.Dispose((TStatusBase)this);
        }

        public abstract void Apply(TStatusBase status);
    }
}