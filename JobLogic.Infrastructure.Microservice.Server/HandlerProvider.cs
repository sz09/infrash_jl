using JobLogic.Infrastructure.Microservice.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server
{
    interface IHandlerProvider
    {
        bool TryGetHandlerInfo(string signature, out HandlerInfo handlerInfo);
        Task<InvocationResult> InvokeTenantlessMiddlewareAsync(MiddlewareContext context);
        Task<InvocationResult> InvokeTenancyMiddlewareAsync(MiddlewareContext context);
    }
    class HandlerProvider : IHandlerProvider
    {
        public HandlerProvider(HandlerConfig tenancyHandlerConfig, HandlerConfig tenantlessHandlerConfig)
        {
            Scan(tenancyHandlerConfig, tenantlessHandlerConfig);

            if (tenancyHandlerConfig != null)
                TenancyHandlerMiddleware = BuildMiddleware(tenancyHandlerConfig.SetupAction, new MainHandlerMiddleware());
            if (tenantlessHandlerConfig != null)
                TenantlessHandlerMiddleware = BuildMiddleware(tenantlessHandlerConfig.SetupAction, new MainHandlerMiddleware());
        }

        IReadOnlyDictionary<string, HandlerInfo> HandlerMap { get; set; }

        HandlerMiddleware TenancyHandlerMiddleware { get; }
        HandlerMiddleware TenantlessHandlerMiddleware { get; }

        HandlerMiddleware BuildMiddleware(Action<MiddlewareBuilder> setupAction, HandlerMiddleware mainHandlerMiddleware)
        {
            var builder = new MiddlewareBuilder();
            setupAction?.Invoke(builder);
            return builder.Build(mainHandlerMiddleware);
        }

        void Scan(HandlerConfig tenancyHandlerConfig, HandlerConfig tenantlessHandlerConfig)
        {
            var handlerList = new List<HandlerInfo>();

            var messageHandlerWithReturnType = typeof(IHandler<,>);
            var messageHandlerType = typeof(IHandler<>);

            if (tenancyHandlerConfig != null)
                ScanTenancy(tenancyHandlerConfig.Assembly);

            if (tenantlessHandlerConfig != null)
                ScanTenantless(tenantlessHandlerConfig.Assembly);

            HandlerMap = handlerList.ToDictionary(x => x.MessageType.FullName, x => x);

            #region inline method
            void ScanTenancy(Assembly assembly)
            {

                var allInterfaces = assembly.GetTypes().SelectMany(x => x.GetInterfaces()).Where(x => x.IsGenericType);

                Type baseMessageType = typeof(TenancyMsg);
                Type baseEventType = typeof(TenancyEvent);

                foreach (var itf in allInterfaces)
                {
                    var itfGenericType = itf.GetGenericTypeDefinition();
                    var messageType = itf.GetGenericArguments()[0];


                    if (itfGenericType == messageHandlerWithReturnType)
                    {
                        bool hasReturn = true;
                        if (baseMessageType.IsAssignableFrom(messageType))
                        {
                            handlerList.Add(new HandlerInfo
                            (itf.GetMethods().Single(), itf, hasReturn, messageType, HandlerMode.Tenancy));
                        }
                    }
                    else if (itfGenericType == messageHandlerType)
                    {
                        bool hasReturn = false;
                        if (baseMessageType.IsAssignableFrom(messageType) || baseEventType.IsAssignableFrom(messageType))
                        {
                            handlerList.Add(new HandlerInfo
                            (itf.GetMethods().Single(), itf, hasReturn, messageType, HandlerMode.Tenancy));
                        }
                    }
                }
            }

            void ScanTenantless(Assembly assembly)
            {
                var allInterfaces = assembly.GetTypes().SelectMany(x => x.GetInterfaces()).Where(x => x.IsGenericType);

                Type baseMessageType = typeof(TenantlessMsg);
                Type baseEventType = typeof(TenantlessEvent);

                foreach (var itf in allInterfaces)
                {
                    var itfGenericType = itf.GetGenericTypeDefinition();
                    var messageType = itf.GetGenericArguments()[0];


                    if (itfGenericType == messageHandlerWithReturnType && baseMessageType.IsAssignableFrom(messageType))
                    {
                        bool hasReturn = true;
                        handlerList.Add(new HandlerInfo
                        (itf.GetMethods().Single(), itf, hasReturn, messageType, HandlerMode.Tenantless));
                    }
                    else if (itfGenericType == messageHandlerType && 
                        (baseMessageType.IsAssignableFrom(messageType) || 
                        baseEventType.IsAssignableFrom(messageType)
                        ))
                    {
                        bool hasReturn = false;
                        handlerList.Add(new HandlerInfo
                        (itf.GetMethods().Single(), itf, hasReturn, messageType, HandlerMode.Tenantless));
                    }
                }
            }
            #endregion



        }

        public bool TryGetHandlerInfo(string signature, out HandlerInfo handlerInfo)
        {
            if (!HandlerMap.ContainsKey(signature))
            {
                handlerInfo = default;
                return false;
            }
            else
            {
                handlerInfo = HandlerMap[signature];
                return true;
            }
        }

        Task<InvocationResult> IHandlerProvider.InvokeTenantlessMiddlewareAsync(MiddlewareContext context)
        {
            return TenantlessHandlerMiddleware.InvokeAsync(context);
        }

        Task<InvocationResult> IHandlerProvider.InvokeTenancyMiddlewareAsync(MiddlewareContext context)
        {
            return TenancyHandlerMiddleware.InvokeAsync(context);
        }
    }

    public class HandlerConfig
    {
        public Assembly Assembly { get; }
        public Action<MiddlewareBuilder> SetupAction { get; }
        public HandlerConfig(Assembly assembly, Action<MiddlewareBuilder> setupAction = null)
        {
            Assembly = assembly;
            SetupAction = setupAction;
        }
    }
}
