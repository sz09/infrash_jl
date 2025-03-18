using JobLogic.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobLogic.Infrastructure.Storage
{
    public static class IoCExtensions
    {
        public static void AddIGlobalPublicStorageClient(this IServiceCollection services, Func<IServiceProvider, GlobalPublicStorageClientSetting> storageConnStringFunc)
        {
            services.AddTransient<IGlobalPublicStorageClient>(x => new GlobalPublicStorageClient(storageConnStringFunc(x)));
        }

        public static void AddIGeneralBlobStorageClient(this IServiceCollection services, Func<IServiceProvider, string> storageConnStringFunc)
        {
            services.AddTransient<IGeneralBlobStorageClient>(x => new GeneralBlobStorageClient(storageConnStringFunc(x)));
        }
    }

    public sealed class GlobalPublicStorageClientSetting
    {
        public GlobalPublicStorageClientSetting(string storageConnString, string customOrigin)
        {
            StorageConnString = storageConnString;
            CustomOrigin = customOrigin;
        }

        public string StorageConnString { get; set; }
        public string CustomOrigin { get; set; }
    }

}
