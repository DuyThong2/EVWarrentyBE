using Microsoft.EntityFrameworkCore;
using PartCatalog.Domain.Models;

namespace PartCatalog.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Part> Parts { get; }
        DbSet<Package> Packages { get; }
        DbSet<Category> Categories { get; }
        DbSet<WarrantyPolicy> WarrantyPolicies { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
