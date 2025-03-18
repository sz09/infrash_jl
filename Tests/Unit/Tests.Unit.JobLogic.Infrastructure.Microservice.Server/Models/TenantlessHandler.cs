using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.Microservice.Server;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public class TenantlessHandler : IHandler<TenantlessMessage, Service<ITenantlessOperationInfo>>
    {
        public Service<ITenantlessOperationInfo> Service { get; set; }
        public TenantlessHandler(Service<ITenantlessOperationInfo> service)
        {
            Service = service;
        }

        public Task<Service<ITenantlessOperationInfo>> HandleAsync(TenantlessMessage input)
        {
            return Task.FromResult(Service);
        }
    }

    public class TenantlessHandlerWithIExecuteService : IHandler<TenantlessMessageWithIExecuteService>
    {
        IExecuteService _executeService;
        public TenantlessHandlerWithIExecuteService(IExecuteService executeService)
        {
            _executeService = executeService;
        }
        public Task HandleAsync(TenantlessMessageWithIExecuteService input)
        {
            return _executeService.DoIt();
        }
    }
}
