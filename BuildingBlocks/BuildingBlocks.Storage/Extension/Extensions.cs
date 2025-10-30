
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using BuildingBlocks.Storage.Bucket;
using BuildingBlocks.Storage.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Storage.Extension
{
    public static class AwsS3Extensions
    {
        public static IServiceCollection AddAwsS3Storage(
            this IServiceCollection services,
            IConfiguration configuration)
        {


            services.Configure<AwsOptions>(
                 configuration.GetSection(nameof(AwsOptions))
             );


            services.AddSingleton<IAwsOptions>(sp =>
                sp.GetRequiredService<IOptions<AwsOptions>>().Value);

            services.AddSingleton<IAmazonS3>(sp =>
            {
                var opts = sp.GetRequiredService<IAwsOptions>();

                if (!string.IsNullOrEmpty(opts.AccessKey) && !string.IsNullOrEmpty(opts.SecretKey))
                {
                    return new AmazonS3Client(
                        opts.AccessKey,
                        opts.SecretKey,
                        RegionEndpoint.GetBySystemName(opts.Region));
                }

                // fallback: IAM Role / Environment / AWS CLI credentials
                return new AmazonS3Client(RegionEndpoint.GetBySystemName(opts.Region));
            });

            // 4️⃣ Đăng ký service S3Storage (scoped)
            services.AddScoped<IS3Storage, S3Storage>();

            return services;
        }
    }
}
