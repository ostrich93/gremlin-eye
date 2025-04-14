using gremlin_eye.Server.Data;
using gremlin_eye.Server.Interfaces.Services;
using gremlin_eye.Server.Entity;
using Microsoft.EntityFrameworkCore;
using gremlin_eye.Server.DTOs;
using System.Threading.Tasks;
using gremlin_eye.Server.Repositories;

namespace gremlin_eye.Server.Services
{
    public class GameLogService : IGameLogService
    {
        private readonly IGameLogRepository _logRepository;
        
        public GameLogService(IGameLogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        
        public async Task<GameLogDTO?> GetGameLogByUser(long gameId, Guid userId)
        {
            GameLog? gameLog = await _logRepository.GetGameLogByUser(gameId, userId);

            if (gameLog == null) return null;

            GameLogDTO logData = new GameLogDTO
            {
                LogId = gameLog.GameLogId,
                GameId = gameId,
                IsPlaying = gameLog.IsPlaying,
                IsBacklog = gameLog.IsBacklog,
                IsWishlist = gameLog.IsWishlist
            };

            if (gameLog.Playthroughs.Any())
            {
                Playthrough playthrough = gameLog.Playthroughs.Last();
                logData.Rating = playthrough.Rating;
                logData.IsPlayed = gameLog.IsPlayed;
                logData.PlayStatus = gameLog.PlayStatus;
            }

            return logData;
        }

        public async Task<GameStatsDTO> GetGameStats(long gameId)
        {
            GameStatsDTO stats = await _logRepository.GetGameStats(gameId);
            stats.AverageRating = _logRepository.GetReviewAverage(gameId);
            stats.RatingCounts = _logRepository.GetReviewCounts(gameId);

            return stats;
        }
    }
}
