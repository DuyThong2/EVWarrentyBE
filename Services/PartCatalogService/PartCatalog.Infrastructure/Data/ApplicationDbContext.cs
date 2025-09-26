using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using PartCatalog.Domain.Models;
using PartCatalog.Application.Data;

namespace PartCatalog.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Part> Parts { get; set; } = null!;
        public DbSet<Package> Packages { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<WarrantyPolicy> WarrantyPolicies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
