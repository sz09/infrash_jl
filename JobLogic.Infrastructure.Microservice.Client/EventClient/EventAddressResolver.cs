using System;
using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public interface IEventAddressResolver
    {
        EventAddress Resolve(BaseMicroserviceEvent microserviceEvent);
    }
    public class EventAddressResolver : IEventAddressResolver
    {
        private readonly string _eventSBNS;
        private readonly string _topicName;
        private readonly IReadOnlyDictionary<string, EventRoute> _customRoute;

        private readonly static string PREFERRED_ROUTE = Environment.GetEnvironmentVariable(EnvironmentVariableName.JL_PREFERRED_ROUTE);

        public EventAddressResolver(string eventSBNS, string topicName, IReadOnlyDictionary<string, EventRoute> customRoute)
        {
            _eventSBNS = eventSBNS;
            _topicName = topicName;
            _customRoute = customRoute;
        }
        public EventAddress Resolve(BaseMicroserviceEvent microserviceEvent)
        {
            var subscribingServiceAttrs = microserviceEvent.GetEventSubscribingServiceAttributes();
            var notationItems = subscribingServiceAttrs.Select(x =>
            {
                var routeName = x.CustomRouteName;
                var route = _customRoute.GetCustomRouteOrNULL(routeName);
                var preferredRoute = _customRoute.GetCustomRouteOrNULL(PREFERRED_ROUTE);
                var suffix = route?.GetApplicableSubscriptionNotationSuffix(x.SubscriptionNotation) ?? preferredRoute?.GetApplicableSubscriptionNotationSuffix(x.SubscriptionNotation);
                return string.Concat(x.SubscriptionNotation, suffix);
            }).ToArray();
            var notation = $"|{string.Join("|", notationItems)}|";
            return new EventAddress(_eventSBNS, _topicName, notation);
        }
    }
}
