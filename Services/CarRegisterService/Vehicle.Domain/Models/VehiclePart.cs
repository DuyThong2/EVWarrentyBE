

namespace Vehicle.Domain.Models
{
    public class VehiclePart
    {
        public Guid PartId { get; set; }
        public Guid VehicleId { get; set; }

        public string Code { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;
        public string? PartType { get; set; }
        public string? BatchCode { get; set; }
        public DateTime? InstalledAt { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public long? WarrantyDistance { get; set; } // km
        public PartStatus Status { get; set; }

        // Navigation
        public Vehicle Vehicle { get; set; } = null!;
        public ICollection<WarrantyHistory> WarrantyHistories { get; set; } = new List<WarrantyHistory>();
    }
}
