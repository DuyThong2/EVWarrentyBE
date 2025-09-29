using Microsoft.EntityFrameworkCore;
using Vehicle.API;
using Vehicle.Infrastructure.Data;
using Vehicle.Infrastructure.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Đăng ký services
builder.Services
    .AddApiServices(builder.Configuration) // API layer (Swagger, Controllers,...)
    .AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
