using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Infrastructure.Data.Configurations
{
    public class TechnicianConfiguration : IEntityTypeConfiguration<Technician>
    {
        public void Configure(EntityTypeBuilder<Technician> b)
        {
            b.ToTable("technicians");

            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasColumnName("technicianId");

            b.Property(x => x.StaffId).HasColumnName("staffId");

            b.Property(x => x.FullName)
                .HasColumnName("fullName")
                .HasMaxLength(100)
                .IsRequired();

            b.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(100);

            b.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(20);

            // Audit columns (không được chỉ rõ trong DBML) → giữ mặc định
        }
    }
}
