using EVWUser.API.Dtos;

namespace EVWUser.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<UserDto> ExtractTokenAsync(string token);
    }
}
