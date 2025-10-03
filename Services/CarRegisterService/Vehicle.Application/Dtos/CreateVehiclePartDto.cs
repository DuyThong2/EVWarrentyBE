using System;
using Vehicle.Domain.Enums;

namespace Vehicle.Application.Dtos
{
    public class CreateVehiclePartDto
    {
        public Guid VehicleId { get; set; }

        public string SerialNumber { get; set; } = null!;
        public string? PartType { get; set; }
        public string? BatchCode { get; set; }
        public DateTime? InstalledAt { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public string? Status { get; set; }
    }
}
