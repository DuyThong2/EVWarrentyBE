using Microsoft.OpenApi.Models;

namespace Vehicle.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vehicle API", Version = "v1" });
            });

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.UseRouting();

            //app.UseHttpsRedirection();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
