using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Vehicle.Infrastructure.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle.Domain.Models.Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle.Domain.Models.Vehicle> b)
        {
            b.ToTable("vehicle");

            b.HasKey(x => x.VehicleId);
            b.Property(x => x.VehicleId).HasColumnName("vehicleId");

            b.Property(x => x.CustomerId).HasColumnName("customerId");

            b.Property(x => x.VIN).HasColumnName("vin").HasMaxLength(17).IsRequired();
            b.HasIndex(x => x.VIN).IsUnique();

            b.Property(x => x.PlateNumber).HasColumnName("plateNumber").HasMaxLength(20);
            b.Property(x => x.Model).HasColumnName("model").HasMaxLength(50);
            b.Property(x => x.Trim).HasColumnName("trim").HasMaxLength(50);
            b.Property(x => x.ModelYear).HasColumnName("modelYear");
            b.Property(x => x.Color).HasColumnName("color").HasMaxLength(30);
            b.Property(x => x.DistanceMeter).HasColumnName("distanceMeter");
            b.Property(x => x.PurchaseDate).HasColumnName("purchaseDate");
            b.Property(x => x.WarrantyStartDate).HasColumnName("warrantyStartDate");
            b.Property(x => x.WarrantyEndDate).HasColumnName("warrantyEndDate");

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasDefaultValue(VehicleStatus.Active)
                .HasConversion(
                    v => v.ToString(),
                    v => (VehicleStatus)Enum.Parse(typeof(VehicleStatus), v));

            b.Property(x => x.CreatedAt).HasColumnName("createdAt");
            b.Property(x => x.UpdatedAt).HasColumnName("updatedAt");

            b.HasIndex(x => x.CustomerId).HasDatabaseName("ix_vehicle_customer");

            b.HasOne(x => x.Customer)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(x => x.CustomerId);
        }
    }
}
