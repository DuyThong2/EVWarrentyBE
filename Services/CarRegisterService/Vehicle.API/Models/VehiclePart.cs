using Vehicle.API.Enums;

namespace Vehicle.API.Models
{
    public class VehiclePart
    {
        public Guid PartId { get; set; }
        public Guid VehicleId { get; set; }

        public string SerialNumber { get; set; } = null!;
        public string? PartType { get; set; }
        public string? BatchCode { get; set; }
        public DateTime? InstalledAt { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public PartStatus Status { get; set; }

        // Navigation
        public Vehicle Vehicle { get; set; } = null!;
    }
}
