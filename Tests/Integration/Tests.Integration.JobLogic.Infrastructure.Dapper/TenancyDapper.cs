using JobLogic.Infrastructure.Dapper;

namespace Tests.Integration.JobLogic.Infrastructure.Dapper
{
    public class TenancyDapper : BaseDbExecutionDapper
    {
        public TenancyDapper(string connectionString, Guid? tenantId = null) : base(connectionString, tenantId)
        {
        }
    }
}