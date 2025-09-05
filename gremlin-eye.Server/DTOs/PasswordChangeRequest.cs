using System.ComponentModel.DataAnnotations;

namespace gremlin_eye.Server.DTOs
{
    public class PasswordChangeRequest
    {
        public Guid? UserId { get; set; }
        public string? ValidationToken { get; set; }

        [Required]
        [MinLength(6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
        public string PasswordConfirmation { get; set; } = "";
    }
}
