using JobLogic.Infrastructure.Microservice.Client;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public class TenantlessTestEvent : TenantlessEvent
    {
    }

    public interface ITenantlessExecuteService
    {
        Task DoIt(Service<ITenantlessOperationInfo> service);
    }

    public class TenantlessEventWithIExecuteService : TenantlessEvent
    {

    }

    //public class TenantlessEventHandler : IEventHandler<TenantlessTestEvent>, IEventHandler<TenantlessAdaptedEvent<TenancyAdapterEvent>>,
    //    IEventHandler<TenantlessAdaptedEvent<TenancyAdapterForTenantlessOnlyEvent>>
    //{

    //    ITenantlessExecuteService _eventHandlerService;
    //    Service<ITenantlessOperationInfo> _service;
    //    public TenantlessEventHandler(ITenantlessExecuteService eventHandlerService, Service<ITenantlessOperationInfo> service)
    //    {
    //        _eventHandlerService = eventHandlerService;
    //        _service = service;
    //    }
    //    public Task HandleEventAsync(TenantlessTestEvent input)
    //    {
    //        return _eventHandlerService.DoIt(_service);
    //    }

    //    public Task HandleEventAsync(TenantlessAdaptedEvent<TenancyAdapterEvent> input)
    //    {
    //        return _eventHandlerService.DoIt(_service);
    //    }

    //    public Task HandleEventAsync(TenantlessAdaptedEvent<TenancyAdapterForTenantlessOnlyEvent> input)
    //    {
    //        return _eventHandlerService.DoIt(_service);
    //    }

    //}

    //public class TenantlessEventHandlerWithIExecuteService : IEventHandler<TenantlessEventWithIExecuteService>, IEventHandler<TenantlessAdaptedEvent<TenancyEventWithIExecuteService>>
    //{
    //    IExecuteService _executeService;
    //    public TenantlessEventHandlerWithIExecuteService(IExecuteService executeService)
    //    {
    //        _executeService = executeService;
    //    }
    //    public Task HandleEventAsync(TenantlessEventWithIExecuteService input)
    //    {
    //        return _executeService.DoIt();
    //    }

    //    public Task HandleEventAsync(TenantlessAdaptedEvent<TenancyEventWithIExecuteService> input)
    //    {
    //        return _executeService.DoIt();
    //    }
    //}
}
