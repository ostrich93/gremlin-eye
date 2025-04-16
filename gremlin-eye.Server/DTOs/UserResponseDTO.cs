using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.DTOs
{
    public class UserResponseDTO
    {
        public string Username { get; set; } = default!;
        public string Token { get; set; } = default!;
        public Guid UserId { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; } = default!;
    }
}
