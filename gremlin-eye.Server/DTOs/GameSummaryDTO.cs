namespace gremlin_eye.Server.DTOs
{
    public class GameSummaryDTO //Used for populating the game cards with minimal necessary information. This is essential for the game/lib/ endpoint, especially when not logged in.
    {
        public long Id { get; set; }
        public string Slug { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? CoverUrl { get; set; } = string.Empty;
        public DateTimeOffset? ReleaseDate { get; set; }
        public double? AverageRating { get; set; }
    }
}
