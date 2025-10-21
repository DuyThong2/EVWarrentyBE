using BuildingBlocks.Messaging.Events;
using MassTransit;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Application.IntegrationEvents.Publishers
{
    public class PartSupplyStatusChangedPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PartSupplyStatusChangedPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync(PartSupply partSupply)
        {
            var @event = new PartSupplyStatusChangedEvent(
                partSupply.PartId ?? Guid.Empty,
                partSupply.Status.ToString()
            );

            await _publishEndpoint.Publish(@event);
        }
    }
}
