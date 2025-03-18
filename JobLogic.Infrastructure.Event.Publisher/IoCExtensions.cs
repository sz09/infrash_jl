using Microsoft.Extensions.DependencyInjection;
using System;

namespace JobLogic.Infrastructure.Event.Publisher
{
    public static class IoCExtensions
    {
        public static void AddEventPublisherClient(this IServiceCollection services, Func<IServiceProvider, PublisherClient> clientFactory)
        {
            services.AddTransient(clientFactory);
            services.AddTransient<ITenancyEventPublisher, TenancyEventPublisher>();
            services.AddTransient<ITenantlessEventPublisher, TenantlessEventPublisher>();
        }
    }

    public class PublisherClient
    {
        public PublisherClient(string sbns, string topicName)
        {
            EventClient = new EventClient(sbns, topicName);
        }

        public virtual EventClient EventClient { get; }
    }
}
