namespace PartCatalog.Application.DTOs;

public class UpdatePartDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? Manufacturer { get; set; }
    public string? Unit { get; set; }
    public string? SerialNumber { get; set; }
    public string? Status { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? PackageId { get; set; }
}
