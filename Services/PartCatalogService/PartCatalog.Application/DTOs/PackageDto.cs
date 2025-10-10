using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCatalog.Application.DTOs
{
    public class PackageDto
    {
        public Guid PackageId { get; set; }
        public string PackageCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Model { get; set; }
        public string? Status { get; set; }
        public decimal Quantity { get; set; }
        public string? Note { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<Guid>? PartId { get; set; }
        public string? PartName { get; set; }
    }

    public class CreatePackageDto
    {
        public string PackageCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Model { get; set; }
        public PartCatalog.Domain.Enums.ActiveStatus? Status { get; set; }
        public decimal Quantity { get; set; } = 1;
        public string? Note { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<Guid>? PartId { get; set; }
    }

    public class UpdatePackageDto
    {
        public Guid PackageId { get; set; }
        public string PackageCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Model { get; set; }
        public PartCatalog.Domain.Enums.ActiveStatus? Status { get; set; }
        public decimal Quantity { get; set; }
        public string? Note { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<Guid>? PartId { get; set; }
    }
}
