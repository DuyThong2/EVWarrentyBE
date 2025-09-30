using Microsoft.EntityFrameworkCore;
using Vehicle.API;
using Vehicle.Application;
using Vehicle.Infrastructure;
using Vehicle.Infrastructure.Data;
using Vehicle.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký Infrastructure services (Database, IApplicationDbContext)
builder.Services.AddInfrastructureServices(builder.Configuration);

// 2. Đăng ký Application services (MediatR, Handlers, Behaviors)
builder.Services.AddApplicationServices(builder.Configuration);

// 3. Đăng ký API services (Controllers, Swagger)
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Swagger + database init khi Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitializeDatabaseAsync();
}

// Middleware + endpoint config
app.UseApiServices();

app.Run();
