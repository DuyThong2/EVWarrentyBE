using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Infrastructure.Data.Configurations
{
    public class PartSupplyConfiguration : IEntityTypeConfiguration<PartSupply>
    {
        public void Configure(EntityTypeBuilder<PartSupply> b)
        {
            b.ToTable("partSuply"); // Giữ nguyên chính tả trong DBML

            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasColumnName("partSuplyId");

            b.Property(x => x.ClaimItemId).HasColumnName("claimItemId");

            b.Property(x => x.PartId).HasColumnName("partId");

            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            b.Property(x => x.NewSerialNumber)
                .HasColumnName("newSerialNumber")
                .HasMaxLength(100);

            b.Property(x => x.ShipmentCode)
                .HasColumnName("shipmentCode")
                .HasMaxLength(50);

            b.Property(x => x.OldPartSerialNumber)
                .HasColumnName("OldPartSerialNumber")
                .HasMaxLength(50);

            b.Property(x => x.ShipmentRef)
                .HasColumnName("shipmentRef")
                .HasMaxLength(80);

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasDefaultValue(SupplyStatus.REQUESTED)
                .HasConversion(
                    s => s.ToString(),
                    dbStatus => (SupplyStatus)Enum.Parse(typeof(SupplyStatus), dbStatus)
                );

            //b.Property(x => x.CreatedAt).HasColumnName("createdAt");

            b.HasOne(x => x.ClaimItem)
                .WithMany(x => x.PartSupplies)
                .HasForeignKey(x => x.ClaimItemId)
                .OnDelete(DeleteBehavior.SetNull);

            
        }
    }
}
