using System;

namespace Vehicle.Application.Filters
{
    public class VehiclePartFilter
    {
        public Guid? PartId { get; set; }
        public Guid? VehicleId { get; set; }
        public string? SerialNumber { get; set; }
        public string? PartType { get; set; }
        public string? BatchCode { get; set; }
        public DateTime? InstalledFrom { get; set; }
        public DateTime? InstalledTo { get; set; }
        public DateTime? WarrantyStartFrom { get; set; }
        public DateTime? WarrantyStartTo { get; set; }
        public DateTime? WarrantyEndFrom { get; set; }
        public DateTime? WarrantyEndTo { get; set; }
        public string? Status { get; set; }
    }
}


