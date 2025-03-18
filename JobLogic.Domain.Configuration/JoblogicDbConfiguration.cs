using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace JobLogic.Domain.Configuration
{
    public class JoblogicDbConfiguration : DbConfiguration
    {
        public JoblogicDbConfiguration(bool enableTransactionFailHandler)
        {
            if (enableTransactionFailHandler)
            {
                SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());
                SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlAzureExecutionStrategy());
            }
        }
    }
}
