using System;
using System.Collections.Generic;

namespace Vehicle.Application.Dtos
{
    public class CustomerVehicleDto
    {
        public Guid VehicleId { get; set; }
        public Guid CustomerId { get; set; }

        public string VIN { get; set; } = string.Empty;
        public string? PlateNumber { get; set; }
        public string? Model { get; set; }
        public string? Trim { get; set; }
        public int ModelYear { get; set; }
        public string? Color { get; set; }
        public long? DistanceMeter { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<VehiclePartDto> Parts { get; set; } = new();
    }
}
