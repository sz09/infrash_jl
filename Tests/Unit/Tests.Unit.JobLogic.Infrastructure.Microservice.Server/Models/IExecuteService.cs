using JobLogic.Infrastructure.Microservice.Client;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public interface IExecuteService
    {
        Task DoIt(Service<ITenancyOperationInfo> service);
        Task DoIt();
    }
}
