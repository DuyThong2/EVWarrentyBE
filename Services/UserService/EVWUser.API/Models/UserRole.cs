namespace EVWUser.API.Models
{
    public class UserRole
    {
        public Guid UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
