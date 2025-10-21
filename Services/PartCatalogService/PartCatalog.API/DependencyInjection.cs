using BuildingBlocks.Exceptions.Handler;
using MassTransit;
using Microsoft.OpenApi.Models;
using PartCatalog.Application.Consumers;

namespace PartCatalog.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Part API", Version = "v1" });
            });
            services.AddControllers();


            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PartSupplyStatusChangedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ReceiveEndpoint("part-supply-status-changed", e =>
                    {
                        e.ConfigureConsumer<PartSupplyStatusChangedConsumer>(context);
                    });
                });
            });


            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.UseRouting();


            app.UseExceptionHandler(options => { });



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

    }
}
