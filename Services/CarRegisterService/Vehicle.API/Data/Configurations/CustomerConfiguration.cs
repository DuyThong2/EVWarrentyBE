using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle.API.Models;
using Vehicle.API.Enums;

namespace Vehicle.API.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> b)
        {
            b.ToTable("customer");

            b.HasKey(x => x.CustomerId);
            b.Property(x => x.CustomerId).HasColumnName("customerId");

            b.Property(x => x.FullName).HasColumnName("fullName").HasMaxLength(100).IsRequired();
            b.Property(x => x.Email).HasColumnName("email").HasMaxLength(100);
            b.Property(x => x.PhoneNumber).HasColumnName("phone").HasMaxLength(20);
            b.Property(x => x.Address).HasColumnName("address").HasMaxLength(200);

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasDefaultValue(CustomerStatus.Active)
                .HasConversion(
                    v => v.ToString(),
                    v => (CustomerStatus)Enum.Parse(typeof(CustomerStatus), v));

            b.Property(x => x.CreatedAt).HasColumnName("createdAt");
            b.Property(x => x.UpdatedAt).HasColumnName("updatedAt");

            b.HasIndex(x => x.Email).HasDatabaseName("ix_customer_email");
            b.HasIndex(x => x.PhoneNumber).HasDatabaseName("ix_customer_phone");
        }
    }
}
