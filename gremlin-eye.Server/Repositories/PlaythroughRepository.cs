using gremlin_eye.Server.Data;
using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Repositories;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class PlaythroughRepository : IPlaythroughRepository
    {
        private readonly DataContext _context;

        public PlaythroughRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Playthrough>> GetGamePlaythroughs(int gameId)
        {
            return await _context.Playthroughs.Include(p => p.GameLog).Include(p => p.PlayLogs).Include(p => p.Review).Where(p => p.GameLog.GameId == gameId).ToListAsync();
        }

        public Task<int> GetGameRatings(long gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PlayLog>> GetPlayLogs(int playthroughId)
        {
            return await _context.PlayLogs.Include(l => l.Playthrough).Where(l => l.Playthrough.PlaythroughId == playthroughId).ToListAsync();
        }

        public async Task<Review?> GetReview(int reviewId)
        {
            return await _context.Reviews.Include(r => r.Playthrough).Include(r => r.Comments).Include(r => r.Likes).Where(r => r.ReviewId == reviewId).FirstOrDefaultAsync();
        }

        public async Task<List<Playthrough>> GetUserPlaythroughs(int gameId, Guid userId)
        {
            return await _context.Playthroughs.Include(p => p.GameLog).Include(p => p.Review).Include(p => p.PlayLogs).Where(p => p.GameLog.GameId == gameId && p.GameLog.UserId == userId).ToListAsync();
        }
    }
}
