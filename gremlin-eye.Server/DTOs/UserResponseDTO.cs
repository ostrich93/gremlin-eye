using gremlin_eye.Server.Enums;

namespace gremlin_eye.Server.DTOs
{
    public class UserResponseDTO
    {
        public string Username { get; set; } = default!;
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; } = default!;
    }
}
