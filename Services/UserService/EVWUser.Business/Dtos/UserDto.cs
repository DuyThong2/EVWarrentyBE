using EVWUser.Data.Enums;

namespace EVWUser.Business.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Status { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
