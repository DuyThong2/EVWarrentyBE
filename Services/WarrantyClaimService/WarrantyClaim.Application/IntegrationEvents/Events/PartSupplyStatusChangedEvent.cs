using BuildingBlocks.Messaging.Events;

namespace WarrantyClaim.Application.IntegrationEvents.Events
{
    public record PartSupplyStatusChangedEvent(
        Guid? PartId,
        string NewStatus
    ) : IntegrationEvent;
}
