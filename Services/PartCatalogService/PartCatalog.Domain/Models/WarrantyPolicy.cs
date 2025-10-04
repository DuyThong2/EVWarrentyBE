using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCatalog.Domain.Models
{
    public class WarrantyPolicy
    {
        public Guid PolicyId { get; set; }
        public Guid PackageId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public PartCatalog.Domain.Enums.PolicyType? Type { get; set; }
        public PartCatalog.Domain.Enums.ActiveStatus? Status { get; set; }
        public string? Description { get; set; }
        public int? WarrantyDuration { get; set; } // months
        public long? WarrantyDistance { get; set; } // km
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Package? Package { get; set; }
    }
}
