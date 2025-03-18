using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server
{
    class MainHandlerMiddleware : HandlerMiddleware
    {
        public MainHandlerMiddleware() : base(null)
        {
        }

        public override async Task<InvocationResult> InvokeAsync(MiddlewareContext middlewareContext)
        {
            var handler = middlewareContext.ServiceProvider.GetRequiredService(middlewareContext.HandlerInfo.HandlerType);
            var task = middlewareContext.HandlerInfo.MethodInfo.Invoke(handler, new object[] { middlewareContext.Message }) as Task;
            await task;
            if (middlewareContext.HandlerInfo.HasReturn)
            {
                return InvocationResult.CreateSuccess(((dynamic)task).Result);
            }
            else
            {
                return InvocationResult.CreateSuccess();
            }
        }
    }
}
