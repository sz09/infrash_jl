using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public interface ICommandAddressResolver
    {
        CommandAddress Resolve(BaseMicroserviceMsg microserviceMsg);
    }
    public class CommandAddressResolver : ICommandAddressResolver
    {
        private readonly static string PREFERRED_ROUTE = Environment.GetEnvironmentVariable(EnvironmentVariableName.JL_PREFERRED_ROUTE);

        private readonly string _commandSBNS;
        private readonly IReadOnlyDictionary<string, MsgCustomRoute> _customRoute;
        private readonly string _defaultQueueName;

        public CommandAddressResolver(string commandSBNS, string defaultQueueName, IReadOnlyDictionary<string, MsgCustomRoute> customRoute)
        {
            _commandSBNS = commandSBNS;
            _customRoute = customRoute;
            _defaultQueueName = defaultQueueName;
        }

        public CommandAddress Resolve(BaseMicroserviceMsg microserviceMsg)
        {
            var routeName = microserviceMsg.GetCustomRouteName();
            var route = _customRoute.GetCustomRouteOrNULL(routeName);
            var preferredRoute = _customRoute.GetCustomRouteOrNULL(PREFERRED_ROUTE);
            var suffix = route?.CommandQueueNameSuffix ?? preferredRoute?.CommandQueueNameSuffix;
            var queueName = _defaultQueueName + suffix;
            return new CommandAddress(_commandSBNS, queueName);
        }
    }
}
