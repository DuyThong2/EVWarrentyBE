using System;

namespace Vehicle.Application.Filters
{
    public class VehicleImageFilter
    {
        public Guid? ImageId { get; set; }
        public Guid? VehicleId { get; set; }
        public string? Url { get; set; }
        public string? Status { get; set; }
    }
}


