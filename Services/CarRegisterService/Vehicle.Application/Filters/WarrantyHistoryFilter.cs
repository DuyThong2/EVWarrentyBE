namespace Vehicle.Application.Filters
{
    public class WarrantyHistoryFilter
    {
        public Guid? VehicleId { get; set; }
        public string? VIN { get; set; }
        public Guid? PartId { get; set; }
        public Guid? ClaimId { get; set; }
        public Guid? PolicyId { get; set; }
        public string? EventType { get; set; } // REPAIR, REPLACEMENT, INSPECTION, EXTENSION
        public string? Description { get; set; }
        public Guid? PerformedBy { get; set; }
        public string? ServiceCenterName { get; set; }
        public string? Status { get; set; } // Active, Completed, Cancelled, Deleted
        public DateTime? WarrantyStartDateFrom { get; set; }
        public DateTime? WarrantyStartDateTo { get; set; }
        public DateTime? WarrantyEndDateFrom { get; set; }
        public DateTime? WarrantyEndDateTo { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public string? SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true;
    }
}
