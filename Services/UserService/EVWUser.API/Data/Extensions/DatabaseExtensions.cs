using Microsoft.EntityFrameworkCore;

namespace EVWUser.API.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedRoleAsync(context);
        }

        private static async Task SeedRoleAsync(UserDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(InitialData.Roles);
                await context.SaveChangesAsync();
            }
        }
    }
}
