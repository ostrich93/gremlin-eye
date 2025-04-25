using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Repositories;
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
            return await _context.GameLogs.Where(l => l.GameId == gameId).GroupBy(l => l.GameId).Select(g => new GameStatsDTO{ 
                PlayedCount = g.Count(i => i.IsPlayed), 
                PlayingCount = g.Count(i => i.IsPlaying),
                BacklogCount = g.Count(i => i.IsBacklog),
                WishlistCount = g.Count(i => i.IsWishlist)
            }).FirstAsync();
        }

        public int[] GetReviewCounts(long gameId)
        {
            return _context.Playthroughs.Where(p => p.GameId == gameId && p.Rating > 0)
                .GroupBy(p => p.Rating).Select(g => g.Count()).ToArray();
        }

        public double GetReviewAverage(long gameId)
        {
            return _context.Playthroughs.Where(p => p.GameId == gameId && p.Rating > 0).Average(p => p.Rating);
        }
    }
}
