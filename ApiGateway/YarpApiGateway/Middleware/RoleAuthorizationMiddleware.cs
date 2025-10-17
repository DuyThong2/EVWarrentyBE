using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace YarpApiGateway.Middleware;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RoleAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        //var endpoint = context.GetEndpoint();
        //if (endpoint == null)
        //{
        //    await _next(context);
        //    return;
        //}

        //var authorizeAttributes = endpoint.Metadata
        //    .GetOrderedMetadata<AuthorizeAttribute>();

        //if (!authorizeAttributes.Any())
        //{
        //    await _next(context);
        //    return;
        //}

        //// Check if user is authenticated
        //if (!context.User.Identity?.IsAuthenticated ?? true)
        //{
        //    context.Response.StatusCode = 401;
        //    return;
        //}

        //// Check roles
        //foreach (var attribute in authorizeAttributes)
        //{
        //    if (!string.IsNullOrEmpty(attribute.Roles))
        //    {
        //        var roles = attribute.Roles.Split(',');
        //        var userRoles = context.User.Claims
        //            .Where(c => c.Type == ClaimTypes.Role)
        //            .Select(c => c.Value);

        //        if (!roles.Any(r => userRoles.Contains(r)))
        //        {
        //            context.Response.StatusCode = 403;
        //            return;
        //        }
        //    }
        //}

        await _next(context);
    }
}