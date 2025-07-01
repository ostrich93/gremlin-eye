using gremlin_eye.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<int> GetGameReviewCount(long gameId)
        {
            return await _context.GameLogs.Where(g => g.GameId == gameId).Include(g => g.Playthroughs.Where(p => p.Review != null)).CountAsync();
            //return await _context.Reviews.CountAsync(r => r.GameId == gameId);
        }
    }
}
