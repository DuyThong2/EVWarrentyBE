using System;

namespace Vehicle.Application.Dtos
{
    public class VehiclePartDto
    {
        public Guid PartId { get; set; }
        public Guid VehicleId { get; set; }

        public string Code { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string? PartType { get; set; }
        public string? BatchCode { get; set; }
        public DateTime? InstalledAt { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public long? WarrantyDistance { get; set; } // km
        public string Status { get; set; } = string.Empty;

        public VehicleDto? Vehicle { get; set; }
    }
}


