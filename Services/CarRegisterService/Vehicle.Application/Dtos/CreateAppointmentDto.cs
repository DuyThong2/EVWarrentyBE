using System;

namespace Vehicle.Application.Dtos
{
    public class CreateAppointmentDto
    {
        public Guid VehicleId { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string? Notes { get; set; }
        public string AppointmentType { get; set; } = "Other";
    }
}
