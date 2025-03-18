using System;
using System.Reflection;

namespace JobLogic.Infrastructure.Microservice.Server
{
    enum HandlerMode
    {
        None,
        Tenancy,
        Tenantless
    }
    class HandlerInfo
    {
        public HandlerInfo(MethodInfo methodInfo, Type handlerType, bool hasReturn, Type messageType, HandlerMode handlerMode)
        {
            MethodInfo = methodInfo;
            HandlerType = handlerType;
            HasReturn = hasReturn;
            MessageType = messageType;
            HandlerMode = handlerMode;
        }

        public MethodInfo MethodInfo { get; }
        public Type HandlerType { get; }
        public bool HasReturn { get; }
        public Type MessageType { get; }
        public HandlerMode HandlerMode { get; }
    }
}
