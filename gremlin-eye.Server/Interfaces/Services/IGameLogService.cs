using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Interfaces.Services
{
    public interface IGameLogService
    {
        public Task<GameLogDTO?> GetGameLogByUser(long gameId, Guid userId);
        public Task<GameStatsDTO> GetGameStats(long gameId);
        
    }
}
