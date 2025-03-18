using System;

namespace JobLogic.Common.Models
{
    public class BaseWebAPIRequest
    {
    }

    public class BaseUserWebAPIRequest : BaseWebAPIRequest
    {
        public Guid UserId { get; set; }
        public bool IsMobileUser { get; set; }
    }

    public class BaseCompanyWebAPIRequest : BaseWebAPIRequest
    {
        public Guid TenantId { get; set; }
        public Guid? WebUserId { get; set; }
    }

    public class BaseMobileWebAPIRequest : BaseWebAPIRequest
    {
        public Guid MobileUserId { get; set; }
        public Guid TenantId { get; set; }

        public int SystemUserId { get; set; }
    }
}
