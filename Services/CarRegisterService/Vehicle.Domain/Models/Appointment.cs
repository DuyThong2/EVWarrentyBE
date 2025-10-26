using Vehicle.Domain.Enums;

namespace Vehicle.Domain.Models
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public Guid VehicleId { get; set; }
        
        // Thông tin lịch hẹn
        public DateTime ScheduledDateTime { get; set; }
        public string? Notes { get; set; }
        
        // Loại lịch hẹn
        public AppointmentType AppointmentType { get; set; }
        
        // Trạng thái
        public AppointmentStatus Status { get; set; }
        
        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        
        // Navigation
        public Vehicle Vehicle { get; set; } = null!;
    }
}
