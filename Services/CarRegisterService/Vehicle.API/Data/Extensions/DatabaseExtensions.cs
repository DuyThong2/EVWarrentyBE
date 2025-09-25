using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vehicle.API.Data;

namespace Vehicle.API.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Đăng ký DbContext với DI container
        /// </summary>
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<VehicleDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));

            return services;
        }

        /// <summary>
        /// Tự động migrate & seed dữ liệu khi khởi động
        /// </summary>
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<VehicleDbContext>();

            // migrate database (tạo bảng nếu chưa có)
            await context.Database.MigrateAsync();

            // vì bạn đã seed bằng HasData trong OnModelCreating,
            // nên EF Core sẽ tự insert dữ liệu sau khi migrate.
            // Nếu muốn seed thêm logic động (ngoài HasData) thì viết ở đây.
        }
    }
}
