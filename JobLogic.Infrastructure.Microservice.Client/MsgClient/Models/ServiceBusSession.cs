namespace JobLogic.Infrastructure.Microservice.Client
{
    public class ServiceBusSession
    {
        public string SessionType { get; private set; }
        public string Id { get; private set; }
        public string SessionId => $"{SessionType}|{Id}";

        private ServiceBusSession() { }
        public static ServiceBusSession Create(string sessionType, string id)
        {
            return new ServiceBusSession
            {
                SessionType = sessionType,
                Id = id
            };
        }
    }
}
