using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IPlaythroughRepository
    {
        public Task<List<Playthrough>> GetGamePlaythroughs(int gameId);
        public Task<List<Playthrough>> GetUserPlaythroughs(int gameId, int userId);
        public Task<Review?> GetReview(int reviewId);
        public Task<List<PlayLog>> GetPlayLogs(int playthroughId);
        public Task<int> GetGameRatings(long gameId);
    }
}
