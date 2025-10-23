namespace BuildingBlocks.Messaging.Events
{
    public record PartSupplyStatusChangedEvent : IntegrationEvent
    {
        public Guid PartId { get; init; }
        public string NewStatus { get; init; } // REQUESTED, SHIPPED, CANCELED, DELIVERED
        public DateTime ChangedAt { get; init; } = DateTime.UtcNow;

        public PartSupplyStatusChangedEvent(Guid partId, string newStatus)
        {
            PartId = partId;
            NewStatus = newStatus;
        }
    }
}
