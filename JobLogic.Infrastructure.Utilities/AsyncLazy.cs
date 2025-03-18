using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Utilities
{
    public class AsyncLazy<T>
    {
        private readonly Lazy<Task<T>> instance;
        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }
    }
}
