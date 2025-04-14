namespace gremlin_eye.Server.DTOs
{
    public class GameStatsDTO
    {
        public int PlayedCount { get; set; }
        public int PlayingCount { get; set; }
        public int BacklogCount { get; set; }
        public int WishlistCount { get; set; }
        public double AverageRating { get; set; }
        public int[] RatingCounts { get; set; } = new int[10];
    }
}
