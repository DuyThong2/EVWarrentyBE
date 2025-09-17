using BuildingBlocks.Messaging.Setting;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;
public static class Extentions
{
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly? assembly = null)
    {


        services.Configure<MessageBrokerSettings>(
            configuration.GetSection(nameof(MessageBrokerSettings))
        );


        services.AddSingleton<IMessageBrokerSettings>(sp =>
            sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
                config.AddConsumers(assembly);

            config.UsingRabbitMq((context, configurator) =>
            {
                var settings = context.GetRequiredService<IMessageBrokerSettings>();

                configurator.Host(new Uri(settings.Host), host =>
                {
                    host.Username(settings.UserName);
                    host.Password(settings.Password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
