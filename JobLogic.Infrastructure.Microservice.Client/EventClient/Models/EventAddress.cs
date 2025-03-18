namespace JobLogic.Infrastructure.Microservice.Client
{
    public sealed class EventAddress
    {
        public string EventSBNS { get; }
        public string SubscribingServicesNotation { get; }
        public string TopicName { get; }

        public EventAddress(string eventSBNS, string topicName, string subscribingServicesNotation)
        {
            EventSBNS = eventSBNS;
            TopicName = topicName;
            SubscribingServicesNotation = subscribingServicesNotation;
        }
    }
}
