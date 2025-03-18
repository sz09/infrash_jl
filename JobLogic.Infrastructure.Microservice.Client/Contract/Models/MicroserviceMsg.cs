namespace JobLogic.Infrastructure.Microservice.Client
{
    public abstract class BaseMicroserviceMsg : BaseMicroserviceData
    {
        protected internal BaseMicroserviceMsg()
        {

        }
    }
    public abstract class TenancyMsg : BaseMicroserviceMsg
    {
    }
    public abstract class TenantlessMsg : BaseMicroserviceMsg
    {
    }
}
