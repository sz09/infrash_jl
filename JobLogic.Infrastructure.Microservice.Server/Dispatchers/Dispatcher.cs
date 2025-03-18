using JobLogic.Infrastructure.Microservice.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server
{

    public interface IDispatcher
    {
        Task<InvocationResult> DispatchAsync(MicroservicePayload microservicePayload);
    }

    class Dispatcher : IDispatcher
    {
        readonly IHandlerProvider _handlerProvider;
        readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider, IHandlerProvider handlerProvider)
        {
            _serviceProvider = serviceProvider;
            _handlerProvider = handlerProvider;
        }

        public virtual async Task<InvocationResult> DispatchAsync(MicroservicePayload microservicePayload)
        {
            if(!_handlerProvider.TryGetHandlerInfo(microservicePayload.MessageSignature, out HandlerInfo handler))
            {
                return InvocationResult.CreateCancelled("Handler not found");
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var message = (microservicePayload.Data as JToken)?.ToObject(handler.MessageType);
                if (message == null)
                    message = microservicePayload.Data;

                var middlewareContext = new MiddlewareContext(scope.ServiceProvider, handler, message, microservicePayload.MessageSignature);
                var scopedMWContextResolver = scope.ServiceProvider.GetRequiredService<RuntimeScopedServiceResolver<IMiddlewareContextDataReader>>();
                scopedMWContextResolver.Value = middlewareContext;
                switch (handler.HandlerMode)
                {
                    case HandlerMode.Tenantless:
                        {
                            var opInfo = microservicePayload.ExtractTenantlessOperationInfo();
                            scope.SetScopedResolver4ITenantlessOperationInfo(opInfo);
                            return await _handlerProvider.InvokeTenantlessMiddlewareAsync(middlewareContext);
                        }
                    case HandlerMode.Tenancy:
                        {
                            var opInfo = microservicePayload.ExtractTenancyOperationInfo();
                            scope.SetScopedResolver4ITenancyOperationInfo(opInfo);
                            scope.SetScopedResolver4ITenantlessOperationInfo(opInfo);
                            return await _handlerProvider.InvokeTenancyMiddlewareAsync(middlewareContext);
                        }
                    default: throw new MicroserviceServerException("Invalid Handler Mode");
                }
            }
        }

        
    }
}
