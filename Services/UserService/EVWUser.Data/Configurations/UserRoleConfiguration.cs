using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EVWUser.Data.Models;

namespace EVWUser.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_role");

            builder.HasKey(ur => ur.UserRoleId);

            builder.Property(ur => ur.UserRoleId).HasColumnName("userRoleId")
                   .HasDefaultValueSql("NEWID()");

            builder.Property(ur => ur.UserId).HasColumnName("userId")
                   .IsRequired();

            builder.Property(ur => ur.RoleId).HasColumnName("roleId")
                   .IsRequired();

            builder.Property(ur => ur.CreatedAt).HasColumnName("createdAt");

            builder.HasOne(ur => ur.User)
                   .WithMany(u => u.UserRoles)
                   .HasForeignKey(ur => ur.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(ur => ur.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
