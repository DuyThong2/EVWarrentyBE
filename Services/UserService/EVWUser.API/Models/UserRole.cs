namespace EVWUser.API.Models
{
    public class UserRole
    {
        public Guid UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
