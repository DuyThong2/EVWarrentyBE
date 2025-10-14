using EVWUser.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EVWUser.Data.Models;

namespace EVWUser.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserId).HasColumnName("userId")
                   .HasDefaultValueSql("NEWID()");

            builder.Property(u => u.Username).HasColumnName("username")
                   .HasMaxLength(100)
                   .IsRequired();
            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.Email).HasColumnName("email")
                   .HasMaxLength(150)
                   .IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Phone).HasColumnName("phone")
                   .HasMaxLength(20);

            builder.Property(u => u.PasswordHash).HasColumnName("passwordHash")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(u => u.AvatarUrl).HasColumnName("avatarUrl")
                   .HasMaxLength(255);

            builder.Property(u => u.Status).HasColumnName("status")
                   .HasDefaultValue(UserStatus.ACTIVE)
                   .HasConversion(
                        s => s.ToString(),
                        dbStatus => (UserStatus)Enum.Parse(typeof(UserStatus), dbStatus))
                   .IsRequired();

            builder.Property(u => u.CreatedAt).HasColumnName("createdAt");

            builder.Property(u => u.UpdatedAt).HasColumnName("updatedAt");

            builder.HasMany(u => u.UserRoles)
                   .WithOne(ur => ur.User)
                   .HasForeignKey(ur => ur.UserId);
        }
    }
}
