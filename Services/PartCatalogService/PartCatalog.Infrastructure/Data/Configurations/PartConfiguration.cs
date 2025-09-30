using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartCatalog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCatalog.Infrastructure.Data.Configurations
{
    public class PartConfiguration : IEntityTypeConfiguration<Part>
    {
        public void Configure(EntityTypeBuilder<Part> builder)
        {
            builder.ToTable("part");
            builder.HasKey(p => p.PartId);
            builder.Property(p => p.PartId).HasColumnName("partId");
            builder.Property(p => p.Name).HasMaxLength(160).IsRequired();
            builder.Property(p => p.Description).HasColumnType("text");
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Manufacturer).HasMaxLength(120);
            builder.Property(p => p.Unit).HasMaxLength(20);
            builder.Property(p => p.SerialNumber).HasMaxLength(100);
            builder.Property(p => p.Status)
                    .HasColumnName("status")
                    .HasDefaultValue(ActiveStatus.ACTIVE) // your enum PartStatus
                    .HasConversion(s => s.ToString(), dbStatus => (ActiveStatus)Enum.Parse(typeof(ActiveStatus), dbStatus));
            builder.HasIndex(p => p.Name).HasDatabaseName("ix_part_name");

            builder.HasOne(p => p.Category).WithMany(c => c.Parts).HasForeignKey(p => p.CateId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Package).WithMany(pk => pk.Parts).HasForeignKey(p => p.PackageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
