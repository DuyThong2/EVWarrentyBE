using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.Storage.Extension;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WarrantyClaim.Application.Extension;

namespace Ordering.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        services.AddAwsS3Storage(configuration);

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(cfg => { }, typeof(CustomMapper));

        // Enable MassTransit for event publishing
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        //services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
