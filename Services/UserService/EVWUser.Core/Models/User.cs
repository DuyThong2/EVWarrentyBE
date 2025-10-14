using EVWUser.Core.Enums;

namespace EVWUser.Core.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = null!;
        public UserStatus Status { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
