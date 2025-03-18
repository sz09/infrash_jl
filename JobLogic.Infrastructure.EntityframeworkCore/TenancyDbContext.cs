using JobLogic.Infrastructure.EntityframeworkCore.TenancyInterceptor;
using Microsoft.EntityFrameworkCore;

namespace JobLogic.Infrastructure.EntityframeworkCore
{
    public abstract class TenancyDbContext : DbContext, ITenancyDbContext
    {
        public Guid TenantId { get; set; }
        public bool IncludedDeletedEntities { get; set; }
        private readonly string ConnectionString;
        public TenancyDbContext(string connectionString, Guid tenantId)
        {
            TenantId = tenantId;
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString, opt =>
            {
                opt.EnableRetryOnFailure();
            });
            optionsBuilder.AddInterceptors(new TenancySessionContextInterceptor());
            base.OnConfiguring(optionsBuilder);
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return this.ProcessSaveChangesAsync(() => OriginalSaveChangesAsync(cancellationToken));
        }
    }
}
