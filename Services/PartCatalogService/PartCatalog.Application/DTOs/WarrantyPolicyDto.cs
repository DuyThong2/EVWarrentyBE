using PartCatalog.Domain.Enums;

namespace PartCatalog.Application.DTOs
{
    public class WarrantyPolicyDto
    {
        public Guid PolicyId { get; set; }
        public Guid PackageId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public PolicyType? Type { get; set; }
        public ActiveStatus? Status { get; set; }
        public string? Description { get; set; }
        public int? WarrantyDuration { get; set; }
        public long? WarrantyDistance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Optional: include package info if needed
        public string? PackageName { get; set; }
    }

    public class CreateWarrantyPolicyDto
    {
        public Guid PackageId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public PolicyType? Type { get; set; }
        public ActiveStatus? Status { get; set; }
        public string? Description { get; set; }
        public int? WarrantyDuration { get; set; }
        public long? WarrantyDistance { get; set; }
    }
}
