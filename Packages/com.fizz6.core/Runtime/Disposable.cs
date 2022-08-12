using System;

namespace Fizz6.Core
{
    public class Disposable : IDisposable
    {
        private readonly Action onDispose;
        
        public Disposable(Action onDispose)
        {
            this.onDispose = onDispose;
        }

        public void Dispose()
        {
            onDispose.Invoke();
        }
    }

    public class Disposable<TValue> : Disposable
    {
        public TValue Value { get; }
        
        public Disposable(Action onDispose, TValue value) : base(onDispose)
        {
            Value = value;
        }
    }
}