namespace BuildingBlocks.Messaging.Events
{
    public record PackageQuantityChangedEvent(
        Guid PackageId,
        decimal OldQuantity,
        decimal NewQuantity
    ) : IntegrationEvent;
}
