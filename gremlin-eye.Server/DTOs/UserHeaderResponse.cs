namespace gremlin_eye.Server.DTOs
{
    public class UserHeaderResponse
    {
        public string UserName { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
