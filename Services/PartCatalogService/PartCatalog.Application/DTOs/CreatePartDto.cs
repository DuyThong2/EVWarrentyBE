namespace PartCatalog.Application.DTOs
{
    public class CreatePartDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? Unit { get; set; }
        public string? SerialNumber { get; set; }
        public string? CategoryId { get; set; }
        public string? Status { get; set; }
    }
}
