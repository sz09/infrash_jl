using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobLogic.Infrastructure.EmailClient
{
    public static class IoCExtensions
    {
        public static void AddEmailClient(this IServiceCollection services, Func<IServiceProvider,ISendGridClient> sendGridClientResolver)
        {
            services.AddTransient<IEmailClient>(x => new EmailClient(sendGridClientResolver(x)));
        }
    }
}
