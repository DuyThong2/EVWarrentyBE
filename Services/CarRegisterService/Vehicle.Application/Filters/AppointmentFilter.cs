using System;

namespace Vehicle.Application.Filters
{
    public class AppointmentFilter
    {
        public Guid? AppointmentId { get; set; }
        public Guid? VehicleId { get; set; }
        public DateTime? ScheduledDateTimeFrom { get; set; }
        public DateTime? ScheduledDateTimeTo { get; set; }
        public string? AppointmentType { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}
