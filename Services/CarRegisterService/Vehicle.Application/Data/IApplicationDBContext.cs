using Microsoft.EntityFrameworkCore;
using Vehicle.Domain.Models;

namespace Vehicle.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<Vehicle.Domain.Models.Vehicle> Vehicles { get; }
        DbSet<VehiclePart> VehicleParts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
