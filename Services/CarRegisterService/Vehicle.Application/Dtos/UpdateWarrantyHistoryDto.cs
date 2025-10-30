namespace Vehicle.Application.Dtos
{
    public class UpdateWarrantyHistoryDto
    {
        public Guid HistoryId { get; set; }
        public Guid? PartId { get; set; }
        public Guid? ClaimId { get; set; }
        public string EventType { get; set; } = "REPAIR"; // REPAIR, REPLACEMENT, INSPECTION, EXTENSION
        public string? Description { get; set; }
        public Guid? PerformedBy { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public long? WarrantyDistance { get; set; } // km
        public string Status { get; set; } = "Active"; // Active, Completed, Cancelled, Deleted
    }
}
