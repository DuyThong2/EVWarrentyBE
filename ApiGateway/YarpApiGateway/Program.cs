using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
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
//builder.Services.AddReverseProxy()
//    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Rate Limiter
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiter =>
    {
        limiter.Window = TimeSpan.FromSeconds(10);
        limiter.PermitLimit = 15;
    });
});





var app = builder.Build();



// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    var swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerSourceSetting>>().Value;

    //c.SwaggerEndpoint(swaggerOptions.Catalog, "Catalog API");
    
});

// CORS
app.UseCors("AllowAll");

// Rate Limiting
app.UseRateLimiter();

// Routing + Metrics endpoint
app.UseRouting();



//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapReverseProxy();
//});

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
