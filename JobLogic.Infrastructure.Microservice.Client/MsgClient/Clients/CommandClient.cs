using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client
{

    

    public class CommandClient
    {
        private readonly ICommandAddressResolver _commandAddressResolver;
        private readonly ICommandSender _commandSender;

        public CommandClient(ICommandAddressResolver commandAddressResolver, ICommandSender commandSender)
        {
            _commandAddressResolver = commandAddressResolver;
            _commandSender = commandSender;
        }

        public Task CommandTenancyHandlerAsync(ITenancyOperationInfo operationInfo, TenancyMsg message, ServiceBusOption serviceBusOption)
        {
            var address = _commandAddressResolver.Resolve(message);
            return _commandSender.SendAsync(address, MicroservicePayload.Create(operationInfo, message), serviceBusOption);
        }

        public Task CommandTenantlessHandlerAsync(ITenantlessOperationInfo operationInfo, TenantlessMsg message, ServiceBusOption serviceBusOption)
        {
            var address = _commandAddressResolver.Resolve(message);
            return _commandSender.SendAsync(address, MicroservicePayload.Create(operationInfo, message), serviceBusOption);
        }

        public Task CommandTenancyHandlerAsync(ITenantlessOperationInfo operationInfo, TenancyMsg message, Guid tenantId, ServiceBusOption serviceBusOption)
        {
            var address = _commandAddressResolver.Resolve(message);
            return _commandSender.SendAsync(address, MicroservicePayload.Create(operationInfo, message, tenantId), serviceBusOption);
        }
    }
}
