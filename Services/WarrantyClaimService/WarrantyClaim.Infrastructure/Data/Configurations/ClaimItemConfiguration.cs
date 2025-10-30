using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Infrastructure.Data.Configurations
{
    public class ClaimItemConfiguration : IEntityTypeConfiguration<ClaimItem>
    {
        public void Configure(EntityTypeBuilder<ClaimItem> b)
        {
            b.ToTable("claimItem");

            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasColumnName("claimItemId");

            b.Property(x => x.ClaimId).HasColumnName("claimId");

            b.Property(x => x.PartSerialNumber)
                .HasColumnName("partSerialNumber")
                .HasMaxLength(100);

            b.Property(x => x.VIN)
                .HasColumnName("VIN")
                .HasMaxLength(100);

            b.Property(x => x.PayAmount)
                .HasColumnName("payAmount")
                .HasPrecision(18, 2);

            b.Property(x => x.PaidBy)
                .HasColumnName("paidBy")
                .HasMaxLength(50);

            b.Property(x => x.Note)
                .HasColumnName("note")
                .HasColumnType("text");

            b.Property(x => x.ImgURLs)
                .HasColumnName("imgURLs")
                .HasColumnType("text");

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasDefaultValue(ClaimItemStatus.PENDING)
                .HasConversion(
                    s => s.ToString(),
                    dbStatus => (ClaimItemStatus) Enum.Parse(typeof(ClaimItemStatus), dbStatus)
                );

            // createdAt (DBML có cột này)
            //b.Property(x => x.CreatedAt).HasColumnName("createdAt");

            // Quan hệ sang PartSupply khai báo ở PartSupplyConfiguration
        }
    }
}
