using gremlin_eye.Server.Enums;

namespace gremlin_eye.Server.DTOs
{
    public class ReviewDTO
    {
        public long ReviewId { get; set; }
        public long GameId { get; set; }
        public long PlaythroughId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string ReviewText { get; set; } = string.Empty;
        public string GameName { get; set; } = string.Empty;
        public string GameSlug { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public bool ContainsSpoilers { get; set; } = false;
        public int? Rating { get; set; }
        public PlatformDTO? Platform { get; set; }
        public int TotalLikes { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public ICollection<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public PlayState PlayStatus { get; set; }

    }
}
