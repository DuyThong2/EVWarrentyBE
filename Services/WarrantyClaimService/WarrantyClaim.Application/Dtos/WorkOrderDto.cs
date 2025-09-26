using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.Dtos
{
    public class WorkOrderDto
    {
        public Guid Id { get; set; }
        public Guid ClaimItemId { get; set; }
        public Guid TechnicianId { get; set; }
        public decimal? WorkingHours { get; set; }
        public string? Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndDate { get; set; }
        public TechnicianDto ? Technician { get; set; }
    }
}
