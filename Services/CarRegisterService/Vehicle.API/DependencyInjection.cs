using Microsoft.OpenApi.Models;
using BuildingBlocks.Exceptions.Handler;

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

            // Add global exception handler
            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.UseRouting();

            // Use global exception handler
            app.UseExceptionHandler();

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
