using System;

namespace Vehicle.Application.Dtos
{
    public class VehicleImageDto
    {
        public Guid ImageId { get; set; }
        public Guid VehicleId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}


