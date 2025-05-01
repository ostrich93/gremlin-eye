using gremlin_eye.Server.DTOs;

namespace gremlin_eye.Server.Services
{
    public interface IGameLogService
    {
        Task<GameLogDTO?> GetGameLogByUser(long gameId, Guid userId);
        Task<GameStatsDTO> GetGameStats(long gameId);
        
    }
}
