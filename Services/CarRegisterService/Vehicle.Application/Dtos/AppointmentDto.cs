using System;

namespace Vehicle.Application.Dtos
{
    public class AppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string? Notes { get; set; }
        public string AppointmentType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public VehicleBriefDto? Vehicle { get; set; }
    }
}
