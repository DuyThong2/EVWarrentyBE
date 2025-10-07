using System;

namespace Vehicle.Domain.Models
{
    public class VehicleImage
    {
        public Guid ImageId { get; set; }
        public Guid VehicleId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public VehicleImageStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Vehicle Vehicle { get; set; } = null!;
    }
}


