namespace EVWUser.Business.Dtos
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
