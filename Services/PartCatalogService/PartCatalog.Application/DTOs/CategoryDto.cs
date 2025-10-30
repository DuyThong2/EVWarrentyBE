using System;

namespace PartCatalog.Application.DTOs
{
    public class CategoryDto
    {
        public Guid CateId { get; set; }
        public string CateCode { get; set; } = null!;
        public string CateName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Quantity { get; set; }
    }
}
