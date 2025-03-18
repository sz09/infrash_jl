using System;

namespace JobLogic.Infrastructure.AzureFunctions
{
    public interface ITenantIdResolver
    {
        Guid TenantId { get; }
    }

    public interface IJlConnStrResolver
    {
        string ConnStr { get; }
    }

    public interface IJobLogicCultureInfoResolver
    {
        JobLogicCultureInfoModel JobLogicCultureInfo { get; }
    }

    public class TenantIdResolver : ITenantIdResolver
    {
        public Guid TenantId { get; set; }
    }
    public interface IUserIdResolver
    {
        Guid? JlWebUserId { get; set; }
    }
    public class CompanyResolver : ITenantIdResolver, IJlConnStrResolver, IJobLogicCultureInfoResolver, IUserIdResolver
    {
        public Guid TenantId { get; set; }

        public string ConnStr { get; set; }

        public JobLogicCultureInfoModel JobLogicCultureInfo { get; set; }
        public Guid? JlWebUserId { get; set; }
    }

    public class Scope : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public class JobLogicCultureInfoModel
    {
        public string TimeFormat { get; set; }
        public string DateFormat { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureName { get; set; }
        public string TimeZoneName { get; set; }
        public string FirstDayOfWeek { get; set; }
    }
}
