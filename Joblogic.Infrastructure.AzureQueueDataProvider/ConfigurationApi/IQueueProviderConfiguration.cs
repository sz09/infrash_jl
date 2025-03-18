namespace JobLogic.Infrastructure.QueueAuditDataProvider.ConfigurationApi
{
    public interface IQueueProviderConfiguration
    {
        IQueueProviderConfiguration ServiceBusConnectionString(string connectionString);

        IQueueProviderConfiguration QueueName(string queueName);
    }
}
