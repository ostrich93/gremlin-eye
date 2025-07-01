namespace gremlin_eye.Server.DTOs
{
    public class ReviewDTO
    {
        public long ReviewId { get; set; }
        public long GameId { get; set; }
        public long PlaythroughId { get; set; }
        public Guid UserId { get; set; }
        public string ReviewText { get; set; } = string.Empty;
        public bool ContainsSpoilers { get; set; } = false;
        public int? Rating { get; set; }

    }
}
