using System;

namespace JobLogic.Infrastructure.Entityframework.TenancyInterceptor
{
    public interface ITenancyDbContext
    {
        Guid TenantId { get; set; }
        bool IncludedDeletedEntities { get; set;  }

    }
}
