using Vehicle.Domain.Enums;

namespace Vehicle.Domain.Models
{
    public class WarrantyHistory
    {
        public Guid HistoryId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid? PartId { get; set; } // có thể null nếu là bảo hành toàn xe
        public Guid? ClaimId { get; set; } // claim từ Warranty Service (nếu có)
        public Guid? PolicyId { get; set; } // tham chiếu warranty_policy bên Part Catalog (nếu có)
        public WarrantyEventType EventType { get; set; }
        public string? Description { get; set; }
        public Guid? PerformedBy { get; set; } // userId hoặc technicianId
        public string? ServiceCenterName { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public WarrantyHistoryStatus Status { get; set; }

        // Navigation
        public Vehicle Vehicle { get; set; } = null!;
        public VehiclePart? Part { get; set; }
    }
}
