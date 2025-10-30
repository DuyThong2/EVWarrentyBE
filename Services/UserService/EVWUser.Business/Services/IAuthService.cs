using EVWUser.Business.Dtos;

namespace EVWUser.Business.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<UserDto> ExtractTokenAsync(string token);
    }
}
