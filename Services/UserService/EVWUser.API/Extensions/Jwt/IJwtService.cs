using EVWUser.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EVWUser.API.Extensions.Jwt
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
        JwtSecurityToken ReadToken(string token);
    }
}
