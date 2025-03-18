using JobLogic.Infrastructure.Entityframework.TenancyInterceptor;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Entityframework
{
    public abstract class TenancyDbContext : DbContext, ITenancyDbContext
    {
        public Guid TenantId { get; set; }
        public bool IncludedDeletedEntities { get; set; }
        static TenancyDbContext()
        {
            EnsureTenancySessionContextInterceptor.RememberToMakeSureDbContextIsFromITenancyDbContext();
        }
        public TenancyDbContext(string connectionString, Guid tenantId) : base(connectionString)
        {
            TenantId = tenantId;
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected int OriginalSaveChanges()
        {
            return base.SaveChanges();
        }

        protected Task<int> OriginalSaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected Task<int> OriginalSaveChangesAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return this.ProcessSaveChanges(OriginalSaveChanges);
        }

        public override Task<int> SaveChangesAsync()
        {
            return this.ProcessSaveChangesAsync(OriginalSaveChangesAsync);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return this.ProcessSaveChangesAsync(() => OriginalSaveChangesAsync(cancellationToken));
        }
    }
}
