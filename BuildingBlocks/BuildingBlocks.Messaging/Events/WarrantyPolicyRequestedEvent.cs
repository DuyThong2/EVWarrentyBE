namespace BuildingBlocks.Messaging.Events
{
    public record WarrantyPolicyRequestedEvent : IntegrationEvent
    {
        public Guid PartId { get; init; }
        public string SerialNumber { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public Guid RequestId { get; init; }
    }
}
