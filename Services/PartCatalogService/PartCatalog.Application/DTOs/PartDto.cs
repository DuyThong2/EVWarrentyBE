namespace PartCatalog.Application.DTOs;

public class PartDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? Manufacturer { get; set; }
    public string? Unit { get; set; }
    public string? SerialNumber { get; set; }
    public string? Status { get; set; }

    public CategoryDto? Category { get; set; }
    public PackageDto? Package { get; set; }
}
