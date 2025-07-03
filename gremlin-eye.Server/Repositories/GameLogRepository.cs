using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class GameLogRepository : IGameLogRepository
    {
        private readonly DataContext _context;
        public GameLogRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<GameLog>> GetAllGameLogs()
        {
            return await _context.GameLogs.Include(gl => gl.Playthroughs).ToListAsync();
        }

        public async Task<List<GameLog>> GetAllLogsByGameId(long gameId)
        {
            return await _context.GameLogs.Include(gl => gl.Playthroughs).ThenInclude(p => p.Review).Where(gl => gl.GameId == gameId).ToListAsync();
        }

        public async Task<GameLog?> GetGameLogByUser(long gameId, Guid userId)
        {
            return await _context.GameLogs.Include(gl => gl.Playthroughs).ThenInclude(gl => gl.Review).FirstOrDefaultAsync(gl => gl.GameId == gameId && gl.UserId == userId);
        }

        public async Task<int> GetPlayedCount(long gameId)
        {
            return await _context.GameLogs.Where(l => l.GameId == gameId).CountAsync(l => l.IsPlayed);
        }
        
        public async Task<int> GetPlayingCount(long gameId)
        {
            return await _context.GameLogs.Where(l => l.GameId == gameId).CountAsync(l => l.IsPlaying);
        }

        public async Task<int> GetBackloggedCount(long gameId)
        {
            return await _context.GameLogs.Where(l => l.GameId == gameId).CountAsync(l => l.IsBacklog);
        }

        public async Task<int> GetWishlistCount(long gameId)
        {
            return await _context.GameLogs.Where(l => l.GameId == gameId).CountAsync(l => l.IsWishlist);
        }

        public async Task<GameStatsDTO> GetGameStats(long gameId)
        {
            var matchingLogs = await _context.GameLogs.Where(l => l.GameId == gameId).ToListAsync();
            if (matchingLogs != null && matchingLogs.Count > 0)
            {
                return new GameStatsDTO
                {
                    PlayedCount = matchingLogs.Count(i => i.IsPlayed),
                    PlayingCount = matchingLogs.Count(i => i.IsPlaying),
                    BacklogCount = matchingLogs.Count(i => i.IsBacklog),
                    WishlistCount = matchingLogs.Count(i => i.IsWishlist)
                };
            } else
            {
                return new GameStatsDTO
                {
                    PlayedCount = 0,
                    PlayingCount = 0,
                    BacklogCount = 0,
                    WishlistCount = 0,
                    AverageRating = 0,
                    RatingCounts = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
                };
            }
        }

        public RatingCount[] GetRatingCounts(long gameId)
        {
            var candidates = _context.GameLogs.Where(g => g.GameId == gameId).SelectMany(g => g.Playthroughs);
            return candidates.Where(p => p.Rating > 0)
                .GroupBy(p => p.Rating).Select(g => 
                    new RatingCount { 
                        Rating = g.Key,
                        Count = g.Count()
                    })
                .ToArray();
        }

        public double GetReviewAverage(long gameId)
        {
            return _context.GameLogs.Where(g => g.GameId == gameId).SelectMany(p => p.Playthroughs).Select(p => p.Rating).DefaultIfEmpty().Average();
            //return _context.Playthroughs.Where(p => p.GameId == gameId && p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average();
        }

        public GameLog GetGameLog(long gameLogId)
        {
            return _context.GameLogs.Include(gl => gl.Playthroughs).Where(gl => gl.Id == gameLogId).First();
        }

        public void Create(GameLog gameLog)
        {
            _context.GameLogs.Add(gameLog);
        }

        public async Task<GameLog?> GetGameLogByUser(string slug, Guid userId)
        {
            return await _context.Games.Where(g => g.Slug == slug).SelectMany(g => g.GameLogs).Where(l => l.UserId == userId).Include(l => l.Playthroughs).FirstOrDefaultAsync();
        }

        public void UpdateGameLog(GameLog gameLog)
        {
            _context.GameLogs.Update(gameLog);
        }

    }
}
