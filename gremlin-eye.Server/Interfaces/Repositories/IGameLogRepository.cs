using gremlin_eye.Server.Models;

namespace gremlin_eye.Server.Repositories
{
    public interface IGameLogRepository
    {
        public Task<List<GameLog>> GetAllGameLogs();
        public Task<List<GameLog>> GetAllLogsByGameId(int gameId);
        public Task<GameLog?> GetGameLogByUser(int gameId, int userId);
        //public Task<GameLog> CreateGameLog(GameLog gameLog);
        //public Task<GameLog> UpdateGameLog(GameLog gameLog);
        //public Task DeleteGameLog(GameLog gameLog);
    }
}
