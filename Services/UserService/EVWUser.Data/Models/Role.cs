namespace EVWUser.Data.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
