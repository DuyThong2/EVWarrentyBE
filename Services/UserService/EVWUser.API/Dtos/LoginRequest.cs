using System.ComponentModel.DataAnnotations;

namespace EVWUser.API.Dtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; } = null!;
        
    }
}
