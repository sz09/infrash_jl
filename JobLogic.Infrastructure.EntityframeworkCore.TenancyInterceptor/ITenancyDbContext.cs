using System;

namespace JobLogic.Infrastructure.EntityframeworkCore.TenancyInterceptor
{
    public interface ITenancyDbContext
    {
        Guid TenantId { get; set; }
        bool IncludedDeletedEntities { get; set;  }

    }
}
