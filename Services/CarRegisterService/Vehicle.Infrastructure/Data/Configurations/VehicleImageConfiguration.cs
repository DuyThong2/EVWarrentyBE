using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle.Domain.Enums;
using Vehicle.Domain.Models;

namespace Vehicle.Infrastructure.Data.Configurations
{
    public class VehicleImageConfiguration : IEntityTypeConfiguration<VehicleImage>
    {
        public void Configure(EntityTypeBuilder<VehicleImage> b)
        {
            b.ToTable("vehicle_image");

            b.HasKey(x => x.ImageId);
            b.Property(x => x.ImageId).HasColumnName("imageId");

            b.Property(x => x.VehicleId).HasColumnName("vehicleId");

            b.Property(x => x.Url).HasColumnName("url").HasMaxLength(500).IsRequired();
            b.Property(x => x.Caption).HasColumnName("caption").HasMaxLength(200);

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasDefaultValue(VehicleImageStatus.Active)
                .HasConversion(
                    s => s.ToString(),
                    s => (VehicleImageStatus)Enum.Parse(typeof(VehicleImageStatus), s));

            b.Property(x => x.CreatedAt).HasColumnName("createdAt");
            b.Property(x => x.UpdatedAt).HasColumnName("updatedAt");

            b.HasIndex(x => x.VehicleId).HasDatabaseName("ix_vehicleimage_vehicle");

            b.HasOne(x => x.Vehicle)
                .WithMany(v => v.Images)
                .HasForeignKey(x => x.VehicleId);
        }
    }
}


