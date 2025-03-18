using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server
{
    public abstract class HandlerMiddleware
    {
        protected HandlerMiddleware Next { get; }
        protected HandlerMiddleware(HandlerMiddleware next)
        {
            Next = next;
        }
        public abstract Task<InvocationResult> InvokeAsync(MiddlewareContext middlewareContext);

    }
}
