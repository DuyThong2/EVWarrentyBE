using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Domain.Models;
using Vehicle.Application.Data;
using Vehicle.Infrastructure.Data;

namespace Vehicle.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle.Domain.Models.Vehicle> Vehicles { get; set; }
        public DbSet<VehiclePart> VehicleParts { get; set; }
        public DbSet<VehicleImage> VehicleImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            // Seed dữ liệu ban đầu (InitialData)
            modelBuilder.SeedInitialData();
            base.OnModelCreating(modelBuilder);
        }
    }
}
