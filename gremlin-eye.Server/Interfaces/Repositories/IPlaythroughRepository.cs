using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IPlaythroughRepository
    {
        Task<List<Playthrough>> GetGamePlaythroughs(int gameId);
        Task<List<Playthrough>> GetUserPlaythroughs(int gameId, Guid userId);
        Task<Review?> GetReview(int reviewId);
        Task<List<PlayLog>> GetPlayLogs(int playthroughId);
        Task<int> GetGameRatings(long gameId);
    }
}
