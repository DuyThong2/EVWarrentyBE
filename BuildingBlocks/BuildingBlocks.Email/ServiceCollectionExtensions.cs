using BuildingBlocks.Email.Abstractions;
using BuildingBlocks.Email.Smtp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Email
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailSender(
            this IServiceCollection services,
            string host,
            int port,
            string username,
            string password,
            bool enableSsl = true)
        {
            services.AddSingleton<IEmailSender>(_ =>
                new SmtpEmailSender(host, port, username, password, enableSsl));

            return services;
        }
    }
}
