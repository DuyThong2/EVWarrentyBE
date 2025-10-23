using Microsoft.AspNetCore.Authorization;

namespace BuildingBlocks.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class HasRolesAttribute : AuthorizeAttribute
    {
        public HasRolesAttribute(params string[] roles)
        {
            if (roles is { Length: > 0 })
                Roles = string.Join(",", roles);
        }
    }
}
