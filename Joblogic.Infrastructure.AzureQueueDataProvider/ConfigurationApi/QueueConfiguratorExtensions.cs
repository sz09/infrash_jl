using Audit.Core;
using Audit.Core.ConfigurationApi;
using System;

namespace JobLogic.Infrastructure.QueueAuditDataProvider.ConfigurationApi
{
    public static class QueueConfiguratorExtensions
    {
        /// <summary>
        /// Azure Queue Config
        /// </summary>
        /// <param name="configurator">Audit config</param>
        /// <param name="serviceBusConnectionString">service bus connection string</param>
        /// <param name="queueName">queue Name</param>
        /// <returns></returns>
        public static ICreationPolicyConfigurator UseAzureQueue(this IConfigurator configurator, string serviceBusConnectionString,
            string queueName = "Audit")
        {
            Configuration.DataProvider = new QueueAuditDataProvider.Provider.QueueAuditDataProvider()
            {
                ServiceBusConnectionString = serviceBusConnectionString,
                QueueName = queueName
                
            };
            return new CreationPolicyConfigurator();
        }
        /// <summary>
        /// Store the events in a Azure Queue database.
        /// </summary>
        /// <param name="configurator">The Audit.NET Configurator</param>
        /// <param name="config">The Azure Queue provider configuration.</param>
        public static ICreationPolicyConfigurator UseAzureQueue(this IConfigurator configurator, Action<IQueueProviderConfiguration> config)
        {
            var azureConfig = new QueueProviderConfiguration();
            config.Invoke(azureConfig);
            return UseAzureQueue(configurator, azureConfig._connectionString, azureConfig._queueName);
        }
    }
}
