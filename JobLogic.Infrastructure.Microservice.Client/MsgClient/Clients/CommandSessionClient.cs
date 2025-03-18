using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public class CommandSessionClient
    {
        private readonly ICommandAddressResolver _commandAddressResolver;
        private readonly ICommandSessionSender _commandSessionSender;

        public CommandSessionClient(ICommandAddressResolver commandAddressResolver, ICommandSessionSender commandSessionSender)
        {
            _commandAddressResolver = commandAddressResolver;
            _commandSessionSender = commandSessionSender;
        }

        public Task CommandTenancyHandlerAsync(ITenancyOperationInfo operationInfo, TenancyMsg message, ServiceBusSession session, ServiceBusOption serviceBusOption)
        {
            var address = _commandAddressResolver.Resolve(message);
            return _commandSessionSender.SendAsync(address, MicroservicePayload.Create(operationInfo, message), session, serviceBusOption);
        }

        public Task CommandTenantlessHandlerAsync(ITenantlessOperationInfo operationInfo, TenantlessMsg message, ServiceBusSession session, ServiceBusOption serviceBusOption)
        {
            var address = _commandAddressResolver.Resolve(message);
            return _commandSessionSender.SendAsync(address, MicroservicePayload.Create(operationInfo, message), session, serviceBusOption);
        }

        public Task CommandTenancyHandlerAsync(ITenantlessOperationInfo operationInfo, TenancyMsg message, Guid tenantId, ServiceBusSession session, ServiceBusOption serviceBusOption)
        {
            var address = _commandAddressResolver.Resolve(message);
            return _commandSessionSender.SendAsync(address, MicroservicePayload.Create(operationInfo, message, tenantId), session, serviceBusOption);
        }
    }
}
