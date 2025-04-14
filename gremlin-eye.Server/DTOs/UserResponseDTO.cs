namespace gremlin_eye.Server.DTOs
{
    public class UserResponseDTO
    {
        public string Username { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string UserId { get; set; }
        public string Role { get; set; } = default!;
    }
}
