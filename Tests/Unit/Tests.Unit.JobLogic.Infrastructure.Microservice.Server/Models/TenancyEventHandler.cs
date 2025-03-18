using JobLogic.Infrastructure.Microservice.Client;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public class TenancyTestEvent : TenancyEvent
    {
    }

    public class TenancyAdapterEvent : TenancyEvent
    {

    }

    public class TenancyAdapterForTenantlessOnlyEvent : TenancyEvent
    {

    }

    public class TenancyEventWithIExecuteService : TenancyEvent
    {

    }

    //public class TenancyEventHandler : IEventHandler<TenancyTestEvent>, IEventHandler<TenancyAdapterEvent>, IEventHandler<TenancyEventWithIExecuteService>
    //{

    //    IExecuteService _tenancyEventHandlerService;
    //    Service<ITenancyOperationInfo> _service;
    //    public TenancyEventHandler(IExecuteService tenancyEventHandlerService, Service<ITenancyOperationInfo> service)
    //    {
    //        _tenancyEventHandlerService = tenancyEventHandlerService;
    //        _service = service;
    //    }
    //    public Task HandleEventAsync(TenancyTestEvent input)
    //    {
    //        return _tenancyEventHandlerService.DoIt(_service);
    //    }

    //    public Task HandleEventAsync(TenancyAdapterEvent input)
    //    {
    //        return _tenancyEventHandlerService.DoIt(_service);
    //    }

    //    public Task HandleEventAsync(TenancyEventWithIExecuteService input)
    //    {
    //        return _tenancyEventHandlerService.DoIt();
    //    }
    //}
}
