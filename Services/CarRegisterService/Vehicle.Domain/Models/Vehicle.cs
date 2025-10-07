

namespace Vehicle.Domain.Models
{
    public class Vehicle
    {
        public Guid VehicleId { get; set; }
        public Guid CustomerId { get; set; }

        public string VIN { get; set; } = null!;
        public string? PlateNumber { get; set; }
        public string? Model { get; set; }
        public string? Trim { get; set; }
        public int ModelYear { get; set; }
        public string? Color { get; set; }
        public long? DistanceMeter { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public VehicleStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Customer Customer { get; set; } = null!;
        public ICollection<VehiclePart> Parts { get; set; } = new List<VehiclePart>();
        public ICollection<VehicleImage> Images { get; set; } = new List<VehicleImage>();
    }
}
