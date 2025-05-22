using gremlin_eye.Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace gremlin_eye.Server.DTOs
{
    public class RegisterUserRequestDTO
    {
        [Required]
        [MaxLength(16)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
        public string? PasswordConfirmation { get; set; }

        public UserRole Role { get; set; } = UserRole.User;
    }
}
