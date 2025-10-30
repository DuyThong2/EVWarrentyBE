namespace Vehicle.Application.Dtos
{
    public class CreateWarrantyHistoryDto
    {
        public Guid VehicleId { get; set; }
        public Guid? PartId { get; set; } // có thể null nếu là bảo hành toàn xe
        public Guid? ClaimId { get; set; } // claim từ Warranty Service (nếu có)
        public string EventType { get; set; } = "REPAIR"; // REPAIR, REPLACEMENT, INSPECTION, EXTENSION
        public string? Description { get; set; }
        public Guid? PerformedBy { get; set; } // userId hoặc technicianId
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public long? WarrantyDistance { get; set; } // km
        public string Status { get; set; } = "Active"; // Active, Completed, Cancelled, Deleted
    }
}
