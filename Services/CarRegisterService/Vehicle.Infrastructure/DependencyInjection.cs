using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vehicle.Application.Data;
using Vehicle.Infrastructure.Data;
using Vehicle.Application.Repositories;
using Vehicle.Infrastructure.Repositories;

namespace Vehicle.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Nếu sau này bạn có interceptor (Audit, DomainEvents) thì mở ra
            // services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            // services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString); 
            });

            // Map interface IApplicationDbContext -> ApplicationDbContext
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // Repositories
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehiclePartRepository, VehiclePartRepository>();
            services.AddScoped<IVehicleImageRepository, VehicleImageRepository>();
            services.AddScoped<IWarrantyHistoryRepository, WarrantyHistoryRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            return services;
        }
    }
}
