using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vehicle.Domain.Models;
using Vehicle.Domain.Enums;
using Vehicle.Application.Data;

namespace Vehicle.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle.Domain.Models.Vehicle> Vehicles { get; set; }
        public DbSet<VehiclePart> VehicleParts { get; set; }
        public DbSet<VehicleImage> VehicleImages { get; set; }
        public DbSet<WarrantyHistory> WarrantyHistories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // ✅ Seed dữ liệu ban đầu
            modelBuilder.SeedInitialData();

            // ✅ Áp dụng global soft-delete filter tự động
            ApplySoftDeleteFilter(modelBuilder);

            // ✅ Mối quan hệ đặc biệt cần lọc theo entity cha
            ApplyParentEntityFilters(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Tự động thêm Global Query Filter cho mọi entity có property 'Status' kiểu Enum có giá trị 'Deleted'
        /// </summary>
        private void ApplySoftDeleteFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                var statusProp = clrType.GetProperty("Status");

                if (statusProp == null || !statusProp.PropertyType.IsEnum)
                    continue;

                if (!Enum.IsDefined(statusProp.PropertyType, "Deleted"))
                    continue;

                var parameter = Expression.Parameter(clrType, "e");
                var property = Expression.Property(parameter, "Status");

                var deletedValue = Enum.Parse(statusProp.PropertyType, "Deleted");
                var deletedConstant = Expression.Constant(deletedValue);

                var notDeleted = Expression.NotEqual(property, deletedConstant);

                // Tạo lambda (e) => e.Status != Deleted
                var lambdaType = typeof(Func<,>).MakeGenericType(clrType, typeof(bool));
                var lambda = Expression.Lambda(lambdaType, notDeleted, parameter);

                modelBuilder.Entity(clrType).HasQueryFilter(lambda);
            }
        }

        /// <summary>
        /// Áp dụng filter đặc biệt để ẩn dữ liệu con khi cha bị soft-delete
        /// </summary>
        private void ApplyParentEntityFilters(ModelBuilder modelBuilder)
        {
            // Vehicle: chỉ hiển thị khi Vehicle và Customer chưa bị xóa
            modelBuilder.Entity<Vehicle.Domain.Models.Vehicle>()
                .HasQueryFilter(v =>
                    v.Status != VehicleStatus.Deleted &&
                    v.Customer.Status != CustomerStatus.Deleted);

            // VehiclePart: chỉ hiển thị khi VehiclePart và Vehicle chưa bị xóa
            modelBuilder.Entity<VehiclePart>()
                .HasQueryFilter(p =>
                    p.Status != PartStatus.Deleted &&
                    p.Vehicle.Status != VehicleStatus.Deleted);

            // VehicleImage: chỉ hiển thị khi VehicleImage và Vehicle chưa bị xóa
            modelBuilder.Entity<VehicleImage>()
                .HasQueryFilter(i =>
                    i.Status != VehicleImageStatus.Deleted &&
                    i.Vehicle.Status != VehicleStatus.Deleted);

            // WarrantyHistory: chỉ hiển thị khi WarrantyHistory và Vehicle chưa bị xóa
            modelBuilder.Entity<WarrantyHistory>()
                .HasQueryFilter(w =>
                    w.Status != WarrantyHistoryStatus.Deleted &&
                    w.Vehicle.Status != VehicleStatus.Deleted);

            // Appointment: chỉ hiển thị khi Appointment và Vehicle chưa bị xóa
            modelBuilder.Entity<Appointment>()
                .HasQueryFilter(a =>
                    a.Status != AppointmentStatus.Deleted &&
                    a.Vehicle.Status != VehicleStatus.Deleted);
        }
    }
}
