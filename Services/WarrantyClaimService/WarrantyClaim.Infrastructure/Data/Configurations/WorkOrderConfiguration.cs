using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WarrantyClaim.Infrastructure.Data.Configurations
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
    {
        public void Configure(EntityTypeBuilder<WorkOrder> b)
        {
            b.ToTable("workorder");

            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasColumnName("workorderId");

            b.Property(x => x.ClaimItemId).HasColumnName("claimItemId"); // → Claim
            b.Property(x => x.TechnicianId).HasColumnName("technicianId");

            b.Property(x => x.WorkingHours)
                .HasColumnName("workingHours")
                .HasPrecision(10, 2);

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasDefaultValue(WorkOrderStatus.OPEN)
                .HasConversion(
                    s => s.ToString(),
                    dbStatus => (WorkOrderStatus)Enum.Parse(typeof(WorkOrderStatus), dbStatus)
                );
                


            b.Property(x => x.StartedAt).HasColumnName("startedAt");
            b.Property(x => x.EndDate).HasColumnName("endDate");

            // Audit (không có cột trong DBML -> giữ mặc định, KHÔNG map tên)
            // Nếu muốn map CreatedAt -> createdAt, thêm b.Property(x => x.CreatedAt).HasColumnName("createdAt");

            b.HasOne(x => x.ClaimItem)
                .WithMany(x => x.WorkOrders)
                .HasForeignKey(x => x.ClaimItemId)
                .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(x => x.Technician)
                .WithMany(x => x.WorkOrders)
                .HasForeignKey(x => x.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
