using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

// Cấu hình Jwt Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                ctx.Response.StatusCode = 401;
                ctx.Response.ContentType = "application/json";
                return ctx.Response.WriteAsync("{\"status\":401,\"message\":\"Unauthorized: Invalid token\"}");
            },
            OnChallenge = ctx =>
            {
                ctx.HandleResponse();
                ctx.Response.StatusCode = 401;
                ctx.Response.ContentType = "application/json";
                return ctx.Response.WriteAsync("{\"status\":401,\"message\":\"Unauthorized: Token is missing or expired\"}");
            },
            OnForbidden = ctx =>
            {
                ctx.Response.StatusCode = 403;
                ctx.Response.ContentType = "application/json";
                return ctx.Response.WriteAsync("{\"status\":403,\"message\":\"Forbidden: You don't have permission to access this resource\"}");
            }
        };
    });

builder.Services.AddAuthorization();

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
