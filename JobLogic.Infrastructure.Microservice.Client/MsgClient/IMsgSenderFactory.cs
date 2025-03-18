using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public interface IMsgSenderFactory
    {
        IRequestSender GetRequestSender();
        ICommandSender GetCommandSender();
        ICommandSessionSender GetCommandSessionSender();
    }
    public interface ICommandSender
    {
        Task SendAsync(CommandAddress address, MicroservicePayload payload, ServiceBusOption serviceBusOption);
    }
    public interface ICommandSessionSender
    {
        Task SendAsync(CommandAddress address, MicroservicePayload payload, ServiceBusSession session, ServiceBusOption serviceBusOption);
    }
    public interface IRequestSender
    {
        Task SendAsync(RequestAddress address, MicroservicePayload payload);
        Task<T> SendAsync<T>(RequestAddress address, MicroservicePayload payload);
    }
    
}
