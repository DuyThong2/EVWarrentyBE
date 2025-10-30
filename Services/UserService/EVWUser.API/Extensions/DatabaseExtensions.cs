using EVWUser.Data;
using EVWUser.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EVWUser.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
            Console.WriteLine("[DB] Migration done.");

            await SeedRoleAsync(context);
            Console.WriteLine("[DB] Role seeding done.");
            await SeedUserAsync(context);
            Console.WriteLine("[DB] User seeding done.");
            await SeedUserRoleAsync(context);
            Console.WriteLine("[DB] UserRole seeding done.");
        }

        private static async Task SeedRoleAsync(UserDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(InitialData.Roles);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUserAsync(UserDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                await context.Users.AddRangeAsync(InitialData.Users);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUserRoleAsync(UserDbContext context)
        {
            if (!await context.UserRoles.AnyAsync())
            {
                await context.UserRoles.AddRangeAsync(InitialData.UserRoles);
                await context.SaveChangesAsync();
            }
        }
    }
}
