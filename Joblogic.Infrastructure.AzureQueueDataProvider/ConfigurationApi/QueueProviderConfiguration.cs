namespace JobLogic.Infrastructure.QueueAuditDataProvider.ConfigurationApi
{
    public class QueueProviderConfiguration: IQueueProviderConfiguration
    {
        internal string _connectionString;
        internal string _queueName;
        public IQueueProviderConfiguration ServiceBusConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        public IQueueProviderConfiguration QueueName(string queueName)
        {
            _queueName = queueName;
            return this;
        }
    }
}
