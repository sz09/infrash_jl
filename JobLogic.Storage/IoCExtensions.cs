using JobLogic.Infrastructure.Contract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JobLogic.Storage
{
    public static class IoCExtensions
    {
        public static void AddStorageHelper(this IServiceCollection services, Func<IServiceProvider,string> cloudStorageConnectionEvalFunc)
        {
            services.AddSingleton<IStorageHelper>(x => new StorageHelper(cloudStorageConnectionEvalFunc(x)));
        }
    }
}
