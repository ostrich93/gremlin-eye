using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IGameLogRepository
    {
        public Task<List<GameLog>> GetAllGameLogs();
        public Task<List<GameLog>> GetAllLogsByGameId(long gameId);
        public Task<GameLog?> GetGameLogByUser(long gameId, Guid userId);
        public Task<int> GetPlayedCount(long gameId);
        public Task<int> GetPlayingCount(long gameId);
        public Task<int> GetBackloggedCount(long gameId);
        public Task<int> GetWishlistCount(long gameId);
        public Task<GameStatsDTO> GetGameStats(long gameId);
        public int[] GetReviewCounts(long gameId);
        public double GetReviewAverage(long gameId);
        //public Task<GameLog> CreateGameLog(GameLog gameLog);
        //public Task<GameLog> UpdateGameLog(GameLog gameLog);
        //public Task DeleteGameLog(GameLog gameLog);
    }
}
