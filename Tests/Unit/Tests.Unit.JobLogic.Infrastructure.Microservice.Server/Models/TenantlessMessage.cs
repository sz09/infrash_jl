using JobLogic.Infrastructure.Microservice.Client;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public class TenantlessMessage : TenantlessMsg, IHasReturn<Service<ITenantlessOperationInfo>>
    {

    }

    public class TenantlessMessageWithIExecuteService : TenantlessMsg
    {

    }
}
