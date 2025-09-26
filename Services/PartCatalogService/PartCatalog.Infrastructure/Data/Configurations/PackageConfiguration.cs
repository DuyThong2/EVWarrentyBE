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
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("package");
            builder.HasKey(p => p.PackageId);
            builder.Property(p => p.PackageCode).HasMaxLength(50).IsRequired();
            builder.HasIndex(p => p.PackageCode).IsUnique();
            builder.Property(p => p.Name).HasMaxLength(160).IsRequired();
            builder.Property(p => p.Description).HasColumnType("text");
            builder.Property(p => p.Model).HasMaxLength(80);
            builder.Property(p => p.Quantity).HasColumnType("decimal(10,2)").IsRequired().HasDefaultValue(1);
            builder.Property(p => p.CreatedAt);
            builder.Property(p => p.UpdatedAt);
            builder.Property(p => p.Note).HasMaxLength(200);
            builder.HasIndex(p => p.Name).HasDatabaseName("ix_package_name");

            builder.HasOne(p => p.Category).WithMany(c => c.Packages).HasForeignKey("CategoryId").OnDelete(DeleteBehavior.SetNull);
        }
    }
}
