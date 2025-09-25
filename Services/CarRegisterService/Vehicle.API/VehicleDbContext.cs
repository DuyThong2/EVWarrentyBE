using Microsoft.EntityFrameworkCore;
using Vehicle.API.Models;

namespace Vehicle.API.Data
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle.API.Models.Vehicle> Vehicles { get; set; }
        public DbSet<VehiclePart> VehicleParts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Áp dụng tất cả configuration trong assembly (CustomerConfiguration, VehicleConfiguration, VehiclePartConfiguration)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VehicleDbContext).Assembly);

            // Seed dữ liệu ban đầu (InitialData)
            modelBuilder.SeedInitialData();

            base.OnModelCreating(modelBuilder);
        }
    }
}
