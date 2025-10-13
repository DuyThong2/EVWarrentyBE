using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle.Domain.Models;
using Vehicle.Domain.Enums;

namespace Vehicle.Infrastructure.Data.Configurations
{
    public class WarrantyHistoryConfiguration : IEntityTypeConfiguration<WarrantyHistory>
    {
        public void Configure(EntityTypeBuilder<WarrantyHistory> builder)
        {
            builder.HasKey(w => w.HistoryId);

            builder.Property(w => w.HistoryId)
                .ValueGeneratedOnAdd();

            builder.Property(w => w.Description)
                .HasMaxLength(1000);

            builder.Property(w => w.ServiceCenterName)
                .HasMaxLength(150);

            builder.Property(w => w.EventType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(w => w.Status)
                .HasConversion<int>()
                .HasDefaultValue(WarrantyHistoryStatus.Active);

            builder.Property(w => w.CreatedAt)
                .IsRequired();

            builder.Property(w => w.UpdatedAt)
                .IsRequired();

            // Foreign key relationships
            builder.HasOne(w => w.Vehicle)
                .WithMany(v => v.WarrantyHistories)
                .HasForeignKey(w => w.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Part)
                .WithMany(p => p.WarrantyHistories)
                .HasForeignKey(w => w.PartId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(w => w.VehicleId);
            builder.HasIndex(w => w.PartId);
            builder.HasIndex(w => w.ClaimId);
            builder.HasIndex(w => w.PolicyId);
            builder.HasIndex(w => w.EventType);
            builder.HasIndex(w => w.Status);
            builder.HasIndex(w => w.PerformedBy);
            builder.HasIndex(w => w.CreatedAt);
        }
    }
}
