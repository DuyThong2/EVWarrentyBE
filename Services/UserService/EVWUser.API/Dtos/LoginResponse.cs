namespace EVWUser.API.Dtos
{
    public class LoginResponse
    {
        public TokenResponse Token { get; set; } = null!;
        public UserDto User { get; set; } = null!;
    }
}
