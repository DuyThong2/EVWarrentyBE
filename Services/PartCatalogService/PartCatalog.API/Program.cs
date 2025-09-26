using PartCatalog.API;
using PartCatalog.Application;
using PartCatalog.Infrastructure;
using PartCatalog.Infrastructure.Data.Extension;

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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Part Catalog API V1");
    });

    await app.InitialiseDatabaseAsync();
}


app.Run();
