using BuildingBlocks.Storage.Settings;
using Microsoft.Extensions.Options;
using Ordering.Application;
using WarrantyClaim.API;
using WarrantyClaim.Infrastructure;
using WarrantyClaim.Infrastructure.Data.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);
var app = builder.Build();




app.MapGet("/", () => "Hello World!");

app.UseApiServices(); // Custom middleware and endpoint config

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Warranty Claim API V1");
    });

    await app.InitialiseDatabaseAsync();
}

//using (var scope = app.Services.CreateScope())
//{
//    var opts = scope.ServiceProvider.GetRequiredService<IOptions<AwsOptions>>().Value;
//    Console.WriteLine($"[DEBUG] AwsOptions loaded:");
//    Console.WriteLine($"Region   = {opts.Region}");
//    Console.WriteLine($"Bucket   = {opts.Bucket}");
//    Console.WriteLine($"KeyPrefix= {opts.KeyPrefix}");
//    Console.WriteLine($"AccessKey= {(string.IsNullOrEmpty(opts.AccessKey) ? "<null>" : "****")}");
//    Console.WriteLine($"SecretKey= {(string.IsNullOrEmpty(opts.SecretKey) ? "<null>" : "****")}");
//}
app.Run();
