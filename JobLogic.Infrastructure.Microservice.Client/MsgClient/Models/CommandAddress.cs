namespace JobLogic.Infrastructure.Microservice.Client
{
    public struct CommandAddress
    {
        public CommandAddress(string sbns, string queueName)
        {
            SBNS = sbns;
            QueueName = queueName;
        }

        public string SBNS { get; }
        public string QueueName { get; }
    }
}
