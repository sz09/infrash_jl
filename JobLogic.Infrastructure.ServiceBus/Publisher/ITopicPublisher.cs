using JobLogic.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.ServiceBus
{
    public interface ITopicPublisher<TMessage>
        where TMessage: BaseServiceBusMessage
    {
        Task SendMessageAsync(TMessage message, CancellationToken cancellationToken = default);
    }
}
