using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCatalog.Domain.Models
{
    public class Package
    {
        public Guid PackageId { get; set; }
        public string PackageCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Model { get; set; }
        public PartCatalog.Domain.Enums.ActiveStatus? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal Quantity { get; set; } = 1;
        public string? Note { get; set; }

        public Guid? CategoryId { get; set; }        // optional: package may belong to a category
        public Category? Category { get; set; }

        public ICollection<Part>? Parts { get; set; }
        public ICollection<WarrantyPolicy>? WarrantyPolicies { get; set; }
    }
}
