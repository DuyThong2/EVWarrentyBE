namespace EVWUser.API.Dtos
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public List<Guid> Roles { get; set; } = new List<Guid>();
    }
}
