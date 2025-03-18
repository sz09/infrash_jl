namespace JobLogic.Infrastructure.Microservice.Client
{
    public abstract class BaseMicroserviceEvent : BaseMicroserviceData
    {
        protected internal BaseMicroserviceEvent()
        {

        }
    }
    public abstract class TenancyEvent : BaseMicroserviceEvent
    {

    }

    public abstract class TenantlessEvent : BaseMicroserviceEvent
    {

    }
}
