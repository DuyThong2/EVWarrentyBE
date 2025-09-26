using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCatalog.Domain.Models
{
    public class Part
    {
        public Guid PartId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? Unit { get; set; }
        public string? SerialNumber { get; set; }
        public ActiveStatus Status { get; set; }

        public Guid? CateId { get; set; }
        public Category? Category { get; set; }

        public Guid? PackageId { get; set; }
        public Package? Package { get; set; }
    }
}
