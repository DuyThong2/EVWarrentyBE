using EVWUser.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace EVWUser.API.Dtos
{
    public class UserUpdateRequest
    {
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        public string? Username { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        public string? AvatarUrl { get; set; }

        [EnumDataType(typeof(UserStatus), ErrorMessage = "Status must be ACTIVE, INACTIVE or LOCKED")]
        public UserStatus? Status { get; set; }

        public List<Guid>? Roles { get; set; } = new List<Guid>();
    }
}
