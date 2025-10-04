using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCatalog.Domain.Models
{
    public class Category
    {
        public Guid CateId { get; set; }
        public string CateCode { get; set; } = null!;
        public string CateName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Quantity { get; set; }

        public ICollection<Package>? Packages { get; set; }
        public ICollection<Part>? Parts { get; set; }
    }
}
