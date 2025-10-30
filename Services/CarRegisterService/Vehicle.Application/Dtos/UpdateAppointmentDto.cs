using System;

namespace Vehicle.Application.Dtos
{
    public class UpdateAppointmentDto
    {
        public DateTime? ScheduledDateTime { get; set; }
        public string? Notes { get; set; }
        public string? AppointmentType { get; set; }
        public string? Status { get; set; }
    }
}
