using System;

namespace Vehicle.Application.Dtos
{
    public class CreateVehicleImageDto
    {
        public Guid VehicleId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public string? Status { get; set; }
    }
}


