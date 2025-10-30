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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.HasKey(c => c.CateId);
            builder.Property(c => c.CateCode).HasMaxLength(50).IsRequired();
            builder.HasIndex(c => c.CateCode).IsUnique();
            builder.Property(c => c.CateName).HasMaxLength(160).IsRequired();
            builder.Property(c => c.Description).HasColumnType("text");
            builder.Property(c => c.Quantity).HasColumnType("decimal(10,2)");
            builder.HasIndex(c => c.CateName).HasDatabaseName("ix_category_name");
        }
    }
}
