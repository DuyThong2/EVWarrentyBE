using Microsoft.EntityFrameworkCore;
using Vehicle.Domain.Enums;
using Vehicle.Domain.Models;
using System;

namespace Vehicle.Infrastructure.Data
{
    public static class InitialData
    {
        public static void SeedInitialData(this ModelBuilder modelBuilder)
        {
            // Dùng giá trị cố định thay vì DateTime.UtcNow
            var fixedNow = new DateTime(2025, 01, 01);

            // Seed Customer
            var customerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = customerId,
                FullName = "Nguyen Van A",
                Email = "nguyenvana@example.com",
                PhoneNumber = "0912345678",
                Address = "Hanoi, Vietnam",
                Status = CustomerStatus.Active, // nếu HasConversion string → để "Active"
                CreatedAt = fixedNow,
                UpdatedAt = fixedNow
            });

            // Seed Vehicle
            var vehicleId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            modelBuilder.Entity<Vehicle.Domain.Models.Vehicle>().HasData(new Vehicle.Domain.Models.Vehicle
            {
                VehicleId = vehicleId,
                CustomerId = customerId,
                VIN = "1HGCM82633A123456",
                PlateNumber = "30A-12345",
                Model = "Honda City",
                Trim = "RS",
                ModelYear = 2022,
                Color = "Black",
                DistanceMeter = 15000,
                PurchaseDate = fixedNow.AddYears(-1),
                WarrantyStartDate = fixedNow.AddYears(-1),
                WarrantyEndDate = fixedNow.AddYears(2),
                Status = VehicleStatus.Active, // nếu HasConversion string → để "Active"
                CreatedAt = fixedNow,
                UpdatedAt = fixedNow
            });

            // Seed VehiclePart
            var partId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
            modelBuilder.Entity<VehiclePart>().HasData(new VehiclePart
            {
                PartId = partId,
                VehicleId = vehicleId,
                SerialNumber = "BAT-123456789",
                PartType = "BATTERY",
                BatchCode = "BATCH-2023",
                InstalledAt = fixedNow.AddMonths(-6),
                WarrantyStartDate = fixedNow.AddMonths(-6),
                WarrantyEndDate = fixedNow.AddYears(1),
                Status = PartStatus.Installed // nếu HasConversion string → để "Installed"
            });
        }
    }
}
