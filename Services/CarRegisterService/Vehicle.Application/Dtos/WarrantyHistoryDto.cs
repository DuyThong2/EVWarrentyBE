namespace Vehicle.Application.Dtos
{
    public class WarrantyHistoryDto
    {
        public Guid HistoryId { get; set; }
        public Guid VehicleId { get; set; }
        public string VIN { get; set; } = null!;
        public Guid? PartId { get; set; }
        public Guid? ClaimId { get; set; }
        public Guid? PolicyId { get; set; }
        public string EventType { get; set; } = null!; // REPAIR, REPLACEMENT, INSPECTION, EXTENSION
        public string? Description { get; set; }
        public Guid? PerformedBy { get; set; }
        public string? ServiceCenterName { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; } = null!; // Active, Completed, Cancelled, Deleted
    }
}
