using Microsoft.Extensions.Logging;
using System;

namespace JobLogic.Infrastructure.Microservice.Server.Host
{
    internal static class LoggerExtensions
    {
        public static void LogDispatchCancelled(this ILogger logger, string message, string serializedMicroservicePayload)
        {
            logger.LogError("Invocation cancelled, message: {0}. Payload to debug: {1}", message, serializedMicroservicePayload);
        }

        public static void LogDispatchException(this ILogger logger, Exception e, string serializedMicroservicePayload)
        {
            logger.LogError(e, "Exception happened. Payload to debug: {0}", serializedMicroservicePayload);
        }
    }
}
