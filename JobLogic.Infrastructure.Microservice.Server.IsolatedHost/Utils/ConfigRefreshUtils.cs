using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System.Linq;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server.IsolatedHost
{
    public static class ConfigRefreshUtils
    {
        public static Task TryRefreshWithFirstRefresherAsync(this IConfigurationRefresherProvider configurationRefresherProvider)
        {
            var refresher = configurationRefresherProvider.Refreshers.First();
            return refresher.TryRefreshAsync();
        }
    }
}
