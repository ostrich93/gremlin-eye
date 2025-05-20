namespace gremlin_eye.Server.DTOs
{
    public class GameDetailsResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public string BannerUrl { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public DateTimeOffset? Date { get; set; }
        public ICollection<PlatformDTO> Platforms { get; set; } = new List<PlatformDTO>();
        public ICollection<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
        public ICollection<CompanyDTO> Companies { get; set; } = new List<CompanyDTO>();
        public SeriesDTO? Series { get; set; }
        public GameSummaryDTO? ParentGame { get; set; }
        public GameLogDTO? GameLog { get; set; } //from user
        public GameStatsDTO? Stats { get; set; }
        public int ListCount { get; set; }
        public int ReviewCount { get; set; }
        public int LikeCount { get; set; }
        public ICollection<ReviewDTO> TopReviews { get; set; } = new List<ReviewDTO>();
        public ICollection<GameSummaryDTO> RelatedGames { get; set; } = new List<GameSummaryDTO>();
    }
}
