using JobLogic.Infrastructure.Microservice.Server;
using System;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public static class Utils
    {
        public static T GetInvocationResponseValue<T>(this InvocationResult invocationResult) where T:class
        {
            return invocationResult.GetResponseOnlyWhenStateIsSuccessWithRespone() as T;
        }
    }
}
