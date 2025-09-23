using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Enums;

namespace WarrantyClaim.Domain.Models
{
    public class WorkOrder : Entity<Guid>
    {
        public Guid ClaimItemId { get; set; }            // Tham chiếu tới ClaimItem (xem ghi chú)
        public Guid TechnicianId { get; set; }
        public decimal? WorkingHours { get; set; }   // decimal(10,2)
        public WorkOrderStatus Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndDate { get; set; }

        // Navs
        public ClaimItem ClaimItem { get; set; } = null!;
        public Technician Technician { get; set; } = null!;
    }
}
