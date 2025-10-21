using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EVWUser.Data.Models;

namespace EVWUser.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("role");

            builder.HasKey(r => r.RoleId);

            builder.Property(r => r.RoleId).HasColumnName("roleId")
                   .HasDefaultValueSql("NEWID()");

            builder.Property(r => r.Name).HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(r => r.Name).IsUnique();

            builder.Property(r => r.Description).HasColumnName("description")
                   .HasMaxLength(250);

            builder.Property(r => r.CreatedAt).HasColumnName("createdAt");

            builder.Property(r => r.UpdatedAt).HasColumnName("updatedAt");

            builder.HasMany(r => r.UserRoles)
                   .WithOne(ur => ur.Role)
                   .HasForeignKey(ur => ur.UserId);
        }
    }
}
