using JobLogic.Infrastructure.Contract;
using Microsoft.Azure.ServiceBus;
using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Event.Publisher
{
    public class EventClient 
    {
        public const string SubscribingServicesNotation_PropertyKey = "SubscribingServicesNotation";
        
        private readonly string _sbns;
        private readonly string _topicName;

        public EventClient(string sbns, string topicName)
        {
            _sbns = sbns;
            _topicName = topicName;
        }
        public Task PublishAsync(ITenancyOperationInfo operationInfo, TenancyEvent tenancyEvent, EventServiceBusOption serviceBusOption)
        {
            var payload = MicroservicePayload.Create(operationInfo, tenancyEvent);
            return SendAsync(payload, serviceBusOption, tenancyEvent.GetEventSubscribingServicesNotation());
        }

        public Task PublishAsync(ITenantlessOperationInfo operationInfo, TenantlessEvent tenantlessEvent, EventServiceBusOption serviceBusOption)
        {
            var payload = MicroservicePayload.Create(operationInfo, tenantlessEvent);
            return SendAsync(payload, serviceBusOption, tenantlessEvent.GetEventSubscribingServicesNotation());
        }

        public Task PublishAsync(ITenantlessOperationInfo operationInfo, TenancyEvent tenancyEvent, Guid tenantId, EventServiceBusOption serviceBusOption)
        {
            var payload = MicroservicePayload.Create(operationInfo, tenancyEvent, tenantId);
            return SendAsync(payload, serviceBusOption, tenancyEvent.GetEventSubscribingServicesNotation());
        }

        public virtual Task SendAsync(MicroservicePayload payload, EventServiceBusOption serviceBusOption, string subscribingServicesNotation)
        {
            var sbmessage = payload.ToMessage(serviceBusOption);
            TopicClient topicClient = TopicClientFactory.For(_sbns, _topicName);
            if (sbmessage.UserProperties.ContainsKey(SubscribingServicesNotation_PropertyKey))
            {
                throw new Exception($"{SubscribingServicesNotation_PropertyKey} is a reserved UserProperty Key");
            }
            else
            {
                sbmessage.UserProperties[SubscribingServicesNotation_PropertyKey] = $"|{string.Join("|", subscribingServicesNotation)}|";
            }
            return topicClient.SendAsync(sbmessage);
        }
    }
}
