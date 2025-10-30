using Microsoft.EntityFrameworkCore;
using Vehicle.Domain.Models;

namespace Vehicle.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<Vehicle.Domain.Models.Vehicle> Vehicles { get; }
        DbSet<VehiclePart> VehicleParts { get; }
        DbSet<VehicleImage> VehicleImages { get; }
        DbSet<WarrantyHistory> WarrantyHistories { get; }
        DbSet<Appointment> Appointments { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
