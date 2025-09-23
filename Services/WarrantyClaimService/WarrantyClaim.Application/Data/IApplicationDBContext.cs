

using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Claim> Claims { get; }
        DbSet<ClaimItem> ClaimItems { get; }
        DbSet<PartSupply> PartSupplies { get; }
        DbSet<WorkOrder> WorkOrders { get; }
        DbSet<Technician> Technicians { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
