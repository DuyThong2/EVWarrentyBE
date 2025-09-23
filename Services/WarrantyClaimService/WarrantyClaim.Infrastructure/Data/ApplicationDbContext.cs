using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Data;
using WarrantyClaim.Domain.Abstractions;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Claim> Claims => Set<Claim>();
        public DbSet<ClaimItem> ClaimItems => Set<ClaimItem>();
        public DbSet<PartSupply> PartSupplies => Set<PartSupply>();
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
        public DbSet<Technician> Technicians => Set<Technician>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Áp dụng toàn bộ IEntityTypeConfiguration<> đã viết
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        // Tự động set CreatedAt / LastModified cho mọi Entity<T>
        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var utcNow = DateTime.UtcNow;

        //    foreach (var entry in ChangeTracker.Entries())
        //    {
        //        if (entry.Entity is IEntity<Guid> eGuid)
        //        {
        //            switch (entry.State)
        //            {
        //                case EntityState.Added:
        //                    // nếu base Entity<T> có CreatedAt/LastModified:
        //                    if (entry.Metadata.FindProperty("CreatedAt") != null)
        //                        entry.CurrentValues["CreatedAt"] = utcNow;
        //                    if (entry.Metadata.FindProperty("LastModified") != null)
        //                        entry.CurrentValues["LastModified"] = utcNow;
        //                    break;

        //                case EntityState.Modified:
        //                    if (entry.Metadata.FindProperty("LastModified") != null)
        //                        entry.CurrentValues["LastModified"] = utcNow;
        //                    // không đụng vào CreatedAt
        //                    break;
        //            }
        //        }
        //    }

        //    return await base.SaveChangesAsync(cancellationToken);
        //}
    }
}

