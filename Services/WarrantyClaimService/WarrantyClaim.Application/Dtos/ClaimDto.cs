using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.Dtos
{
    public class ClaimDto
    {
        public Guid Id { get; set; }
        public Guid? StaffId { get; set; }
        public string VIN { get; set; } = string.Empty;
        public long? DistanceMeter { get; set; }
        public string? Description { get; set; }
        public string? FileURL { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? ClaimType { get; set; }
        public string? Status { get; set; }
        public List<ClaimItemDto> Items { get; set; } = new();

        public TechnicianDto ? Technician { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
