using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using YarpApiGateway.Middleware;
using YarpApiGatway.Settings;



var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SwaggerSourceSetting>(
    builder.Configuration.GetSection(SwaggerSourceSetting.SectionName)
);

// CORS 



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// Reverse Proxy (YARP)
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Rate Limiter
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiter =>
    {
        limiter.Window = TimeSpan.FromSeconds(10);
        limiter.PermitLimit = 15;
    });
});

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["SecretKey"] ?? ""))
        };
    });


builder.Services.AddAuthorization();



var app = builder.Build();



// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    var swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerSourceSetting>>().Value;

    c.SwaggerEndpoint(swaggerOptions.WarrantyClaim, "Warrenty Claim API");
    c.SwaggerEndpoint(swaggerOptions.VehicleClaim, "Vehicle API");
    c.SwaggerEndpoint(swaggerOptions.PartCatalog, "Part Catalog API");
    c.SwaggerEndpoint(swaggerOptions.User, "User API");

});

// CORS
app.UseCors("AllowAll");

// Rate Limiting
app.UseRateLimiter();

// Routing + Metrics endpoint
app.UseRouting();

// Authentication + Authorization
app.UseAuthentication();
app.UseMiddleware<RoleAuthorizationMiddleware>(); 
app.UseAuthorization();


app.MapReverseProxy();


// Optional: root test
app.MapGet("/", () =>
{
    var swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerSourceSetting>>().Value;
    return swaggerOptions;
});

app.MapGet("/api", (IOptions<SwaggerSourceSetting> opt) => Results.Json(opt.Value));

// 1) Health
app.MapGet("/healthz", () => Results.Ok("OK"));

// 2) Env + hostname
app.MapGet("/_gw/env", () =>
{
    var body = new
    {
        Hostname = Dns.GetHostName(),
        Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
        HttpPorts = Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS"),
        HttpsPorts = Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORTS"),
        Time = DateTimeOffset.UtcNow
    };
    return Results.Json(body);
});









app.Run();
