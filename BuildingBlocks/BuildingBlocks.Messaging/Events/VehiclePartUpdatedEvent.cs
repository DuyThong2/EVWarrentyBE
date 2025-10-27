
namespace BuildingBlocks.Messaging.Events
{
    public record VehiclePartUpdatedEvent : IntegrationEvent
    {
        public Guid PartId { get; init; }
        public Guid? VehicleId { get; init; }
        public string SerialNumber { get; init; } = string.Empty;
        public string? OldPartSerialNumber { get; init; } // Thêm field mới
        public string? PartType { get; init; }
        public string? BatchCode { get; init; }
        public DateTime? InstalledAt { get; init; }
        public DateTime? WarrantyStartDate { get; init; }
        public DateTime? WarrantyEndDate { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? ShipmentCode { get; init; }
        public string? ShipmentRef { get; init; }
        public Guid? ClaimId { get; init; } // Thêm ClaimId
        public Guid? PerformedBy { get; init; } // Thêm PerformedBy (StaffId)
    }
}
