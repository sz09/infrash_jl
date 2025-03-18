using JobLogic.Infrastructure.Microservice.Client;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Server
{
    public interface IHandler<I>
    {
        Task HandleAsync(I input);
    }

    public interface IHandler<I,O> where I: IHasReturn<O>
    {
        Task<O> HandleAsync(I input);
    }
}
