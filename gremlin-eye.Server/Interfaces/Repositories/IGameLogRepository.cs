﻿using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IGameLogRepository
    {
        Task<List<GameLog>> GetAllGameLogs();
        Task<List<GameLog>> GetAllLogsByGameId(long gameId);
        Task<GameLog?> GetGameLogByUser(long gameId, Guid userId);
        Task<GameLog?> GetGameLogByUser(string slug, Guid userId);
        Task<GameLog?> GetTargetGameLog(long logId, Guid userId);
        Task<int> GetPlayedCount(long gameId);
        Task<int> GetPlayingCount(long gameId);
        Task<int> GetBackloggedCount(long gameId);
        Task<int> GetWishlistCount(long gameId);
        void Create(GameLog gameLog);
        Task<GameStatsDTO> GetGameStats(long gameId);
        GameLog GetGameLog(long gameLogId);
        RatingCount[] GetRatingCounts(long gameId);
        double GetReviewAverage(long gameId);
        
        void UpdateGameLog(GameLog gameLog);
        //Task DeleteGameLog(GameLog gameLog);
    }
}
