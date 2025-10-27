using EVWUser.Data.Models;

namespace EVWUser.Data.Extensions
{
    public class InitialData
    {
        public static IEnumerable<Role> Roles =>
        new List<Role>
        {
            new Role
            {
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Service Center Staff",
                Description = "Staff member working at the service center",
            },
            new Role
            {
                RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Service Center Technician",
                Description = "Technician responsible for repairs and maintenance",
            },
            new Role
            {
                RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "EVM Staff",
                Description = "Staff member working with Electric Vehicle Management",
            },
            new Role
            {
                RoleId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Admin",
                Description = "Administrator with full access",
            }
        };

        public static IEnumerable<User> Users =>
        new List<User>
        {
            new User
            {
                UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Username = "admin",
                Email = "admin@gmail.com",
                PasswordHash = "AQAAAAEAACcQAAAAEO61Q6Ij9hZ0h7Jmu7Lz7cX1W6bUij6rT29FqUa4QIxLuKJ0PNQRPx6C+23kY/JJ4A==",
                Status = Enums.UserStatus.ACTIVE,
                AvatarUrl = "https://png.pngtree.com/png-clipart/20240917/original/pngtree-administrator-admin-avatar-png-image_16031562.png"
            }
        };

        public static IEnumerable<UserRole> UserRoles =>
        new List<UserRole>
        {
            new UserRole
            {
                UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                RoleId = Guid.Parse("44444444-4444-4444-4444-444444444444") // Admin role
            }
        };
    }
}
