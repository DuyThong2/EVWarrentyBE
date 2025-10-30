using System;

namespace Vehicle.Application.Dtos
{
    public class VehicleBriefDto
    {
        public Guid VehicleId { get; set; }
        public string VIN { get; set; } = string.Empty;
        public string? PlateNumber { get; set; }
        public string? Model { get; set; }
    }
}
