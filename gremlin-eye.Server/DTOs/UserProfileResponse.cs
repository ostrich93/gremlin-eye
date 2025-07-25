namespace gremlin_eye.Server.DTOs
{
    public class UserProfileResponse
    {
        public string Username { get; set; } = string.Empty;
        public int TotalGamesPlayed { get; set; }
        public int GamesPlayedThisYear { get; set; }
        public int GamesBacklogged { get; set; }
        public int[] RatingCounts { get; set; } = new int[10];
        public List<ReviewDTO> RecentReviews { get; set; } = [];
    }
}
