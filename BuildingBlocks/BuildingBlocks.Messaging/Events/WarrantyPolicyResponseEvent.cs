namespace BuildingBlocks.Messaging.Events
{
    public record WarrantyPolicyResponseEvent : IntegrationEvent
    {
        public Guid RequestId { get; init; }
        public Guid PartId { get; init; }
        public int WarrantyDuration { get; init; } // Tháng
        public int WarrantyDistance { get; init; } // Km
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
