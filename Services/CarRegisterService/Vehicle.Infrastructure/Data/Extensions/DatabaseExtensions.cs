using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vehicle.Infrastructure.Data;

namespace Vehicle.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Tự động migrate & seed dữ liệu khi khởi động
        /// </summary>
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // migrate database (tạo bảng nếu chưa có)
            await context.Database.MigrateAsync();

            // Nếu có seed thêm ngoài HasData thì viết thêm ở đây
        }
    }
}
