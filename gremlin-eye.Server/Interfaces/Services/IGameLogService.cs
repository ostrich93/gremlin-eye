using gremlin_eye.Server.Models;

namespace gremlin_eye.Server.Interfaces.Services
{
    public interface IGameLogService
    {
        IEnumerable<GameLog> GetAllGameLogs();
        IEnumerable<GameLog> GetAllLogsByGameId(int gameId);
        GameLog? GetGameLogByUser(int gameId, int userId);
        IEnumerable<GameLog> GetPlayedLogsByUser(int userId);
        IEnumerable<GameLog> GetPlayedLogsByGame(int gameId);
        IEnumerable<GameLog> GetPlayingLogsByUser(int userId);
        IEnumerable<GameLog> GetPlayingLogsByGame(int gameId);
        IEnumerable<GameLog> GetBackloggedByUser(int userId);
        IEnumerable<GameLog> GetBackloggedByGame(int gameId);
        IEnumerable<GameLog> GetWishlistByUser(int userId);
        IEnumerable<GameLog> GetWishlistByGame(int gameId);
    }
}
