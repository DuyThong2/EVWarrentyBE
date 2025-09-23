using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Infrastructure.Data.Configurations
{
    public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> b)
        {
            b.ToTable("claim");

            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasColumnName("claimId");

            b.Property(x => x.StaffId).HasColumnName("staffId");

            b.Property(x => x.VIN)
                .HasColumnName("VIN")
                .HasMaxLength(17)
                .IsRequired();

            b.Property(x => x.DistanceMeter)
                .HasColumnName("distanceMeter");

            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            b.Property(x => x.FileURL)
                .HasColumnName("fileURL")
                .HasColumnType("text");

            b.Property(x => x.TotalPrice)
                .HasColumnName("totalPrice")
                .HasPrecision(18, 2);

            b.Property(x => x.ClaimType)
                .HasColumnName("claimType")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasDefaultValue(ClaimStatus.SUBMITTED)
                .HasConversion(
                    s => s.ToString(),
                    dbStatus => (ClaimStatus)Enum.Parse(typeof(ClaimStatus), dbStatus)
                );

            // Map audit columns to DBML names
            //b.Property(x => x.CreatedAt).HasColumnName("createdAt");
            //b.Property(x => x.LastModified).HasColumnName("updatedAt");

            // Relationships
            b.HasMany(x => x.Items)
                .WithOne(x => x.Claim)
                .HasForeignKey(x => x.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            //b.HasMany(x => x.WorkOrders)
            //    .WithOne(x => x.Claim)
            //    .HasForeignKey(x => x.ClaimId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
