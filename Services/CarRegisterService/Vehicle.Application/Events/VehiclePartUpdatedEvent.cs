using BuildingBlocks.Messaging.Events;

namespace Vehicle.Application.Events
{
    public record VehiclePartUpdatedEvent : IntegrationEvent
    {
        public Guid PartId { get; init; }
        public Guid? VehicleId { get; init; }
        public string SerialNumber { get; init; } = string.Empty;
        public string? PartType { get; init; }
        public string? BatchCode { get; init; }
        public DateTime? InstalledAt { get; init; }
        public DateTime? WarrantyStartDate { get; init; }
        public DateTime? WarrantyEndDate { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? ShipmentCode { get; init; }
        public string? ShipmentRef { get; init; }
    }
}
