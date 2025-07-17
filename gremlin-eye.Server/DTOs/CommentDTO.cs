namespace gremlin_eye.Server.DTOs
{
    public class CommentDTO
    {
        public long CommentId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string CommentBody { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CommentRequest
    {
        public string CommentBody { get; set; } = string.Empty;
    }
}
