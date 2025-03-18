using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobLogic.Infrastructure.OData.Server
{
    public static class IoCExtensions
    {
        public static void AddODataServer(this IServiceCollection services, Func<IEdmModel> builderFunc)
        {
            services.AddSingleton<IEdmModelProvider>(x => new EdmModelProvider(builderFunc()));
            services.AddTransient<IODataResolver, ODataResolver>();
        }
    }
}
