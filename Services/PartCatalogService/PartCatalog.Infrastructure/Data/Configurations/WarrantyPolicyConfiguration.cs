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
    public class WarrantyPolicyConfiguration : IEntityTypeConfiguration<WarrantyPolicy>
    {
        public void Configure(EntityTypeBuilder<WarrantyPolicy> builder)
        {
            builder.ToTable("warranty_policy");
            builder.HasKey(w => w.PolicyId);
            builder.Property(w => w.Code).HasMaxLength(50).IsRequired();
            builder.HasIndex(w => w.Code).IsUnique();
            builder.Property(w => w.Name).HasMaxLength(160).IsRequired();
            builder.Property(w => w.Description).HasColumnType("text");
            builder.Property(w => w.WarrantyDistance).HasColumnType("bigint");
            builder.Property(w => w.WarrantyDuration);
            builder.Property(w => w.CreatedAt);
            builder.Property(w => w.UpdatedAt);
            builder.HasIndex(w => new { w.PackageId, w.Status }).HasDatabaseName("ix_policy_pkg_status");

            builder.HasOne(w => w.Package).WithMany(p => p.WarrantyPolicies).HasForeignKey(w => w.PackageId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
