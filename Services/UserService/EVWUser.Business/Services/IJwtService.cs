using EVWUser.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EVWUser.Business.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
        JwtSecurityToken ReadToken(string token);
    }
}
