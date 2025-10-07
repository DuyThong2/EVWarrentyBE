using System;

namespace Vehicle.Application.Filters
{
    public class VehicleFilter
    {
        public Guid? VehicleId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? VIN { get; set; }
        public string? PlateNumber { get; set; }
        public string? Model { get; set; }
        public string? Trim { get; set; }
        public int? ModelYear { get; set; }
        public string? Color { get; set; }
        public string? Status { get; set; }

        public DateTime? PurchaseDateFrom { get; set; }
        public DateTime? PurchaseDateTo { get; set; }
        public DateTime? WarrantyStartFrom { get; set; }
        public DateTime? WarrantyStartTo { get; set; }
        public DateTime? WarrantyEndFrom { get; set; }
        public DateTime? WarrantyEndTo { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}


