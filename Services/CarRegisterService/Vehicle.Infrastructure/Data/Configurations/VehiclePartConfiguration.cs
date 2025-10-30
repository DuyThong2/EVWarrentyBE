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
    public class VehiclePartConfiguration : IEntityTypeConfiguration<VehiclePart>
    {
        public void Configure(EntityTypeBuilder<VehiclePart> b)
        {
            b.ToTable("vehicle_part");

            b.HasKey(p => p.PartId);
            b.Property(p => p.PartId).HasColumnName("partId");

            b.Property(p => p.VehicleId).HasColumnName("vehicleId");

            b.Property(p => p.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            b.Property(p => p.SerialNumber)
                .HasColumnName("serialNumber")
                .HasMaxLength(100)
                .IsRequired();

            b.HasIndex(p => p.SerialNumber).IsUnique();

            b.Property(p => p.PartType)
                .HasColumnName("partType")
                .HasMaxLength(50);

            b.Property(p => p.BatchCode)
                .HasColumnName("batchCode")
                .HasMaxLength(50);

            b.Property(p => p.InstalledAt).HasColumnName("installedAt");
            b.Property(p => p.WarrantyStartDate).HasColumnName("warrantyStartDate");
            b.Property(p => p.WarrantyEndDate).HasColumnName("warrantyEndDate");
            b.Property(p => p.WarrantyDistance).HasColumnName("warrantyDistance");

            b.Property(p => p.Status)
                .HasColumnName("status")
                .HasConversion(
                    s => s.ToString(),
                    s => (PartStatus)Enum.Parse(typeof(PartStatus), s))
                .HasDefaultValue(PartStatus.Installed);

            // Indexes
            b.HasIndex(p => p.VehicleId).HasDatabaseName("ix_part_vehicle");
            b.HasIndex(p => p.PartType).HasDatabaseName("ix_part_parttype");

            // FK
            b.HasOne(p => p.Vehicle)
             .WithMany(v => v.Parts)
             .HasForeignKey(p => p.VehicleId);
        }
    }
}
