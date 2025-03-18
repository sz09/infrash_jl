using JobLogic.Infrastructure.Microservice.Server;
using System;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server.Tests
{
    class SimpleForwardMiddleware : HandlerMiddleware
    {
        public static event Action<MiddlewareContext> OnBeforeInvoke;
        public SimpleForwardMiddleware(HandlerMiddleware next):base(next)
        {

        }
        public override Task<InvocationResult> InvokeAsync(MiddlewareContext middlewareContext)
        {
            OnBeforeInvoke?.Invoke(middlewareContext);
            return Next.InvokeAsync(middlewareContext);
        }
    }

    class FailedInvocationMiddleware : HandlerMiddleware
    {
        public FailedInvocationMiddleware(HandlerMiddleware next) : base(next)
        {

        }
        public override Task<InvocationResult> InvokeAsync(MiddlewareContext middlewareContext)
        {
            return Task.FromResult(InvocationResult.CreateCancelled("failed by test"));
        }
    }

    class Middleware1 : HandlerMiddleware
    {
        public static event Action OnBeforeInvoke;
        public static event Action OnAfterInvoke;
        public Middleware1(HandlerMiddleware next) : base(next)
        {

        }
        public override async Task<InvocationResult> InvokeAsync(MiddlewareContext middlewareContext)
        {
            OnBeforeInvoke?.Invoke();
            var rs = await Next.InvokeAsync(middlewareContext);
            OnAfterInvoke?.Invoke();
            return rs;
        }
    }

    class Middleware2 : HandlerMiddleware
    {
        public static event Action OnBeforeInvoke;
        public static event Action OnAfterInvoke;
        public Middleware2(HandlerMiddleware next) : base(next)
        {

        }
        public override async Task<InvocationResult> InvokeAsync(MiddlewareContext middlewareContext)
        {
            OnBeforeInvoke?.Invoke();
            var rs = await Next.InvokeAsync(middlewareContext);
            OnAfterInvoke?.Invoke();
            return rs;
        }
    }
}
