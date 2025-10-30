using System;

namespace Vehicle.Application.Dtos
{
    public class CreateVehicleDto
    {
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
        public string? Status { get; set; }
    }
}
