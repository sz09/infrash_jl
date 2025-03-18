using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.Microservice.Server;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public class TenancyTestMessage : TenancyMsg, IHasReturn<Service<ITenancyOperationInfo>>
    {

    }

    public class TenancyMessageWithIExecuteService : TenancyMsg
    {

    }

    public class TenancyMessageHandler : IHandler<TenancyTestMessage, Service<ITenancyOperationInfo>>
    {
        public Service<ITenancyOperationInfo> Service { get; set; }
        public TenancyMessageHandler(Service<ITenancyOperationInfo> service)
        {
            Service = service;
        }

        public Task<Service<ITenancyOperationInfo>> HandleAsync(TenancyTestMessage input)
        {
            return Task.FromResult(Service);
        }
    }

    public class TenancyMessageHandlerWithIExecuteService : IHandler<TenancyMessageWithIExecuteService>
    {
        IExecuteService _executeService;
        public TenancyMessageHandlerWithIExecuteService(IExecuteService executeService)
        {
            _executeService = executeService;
        }
        public Task HandleAsync(TenancyMessageWithIExecuteService input)
        {
            return _executeService.DoIt();
        }
    }
}
