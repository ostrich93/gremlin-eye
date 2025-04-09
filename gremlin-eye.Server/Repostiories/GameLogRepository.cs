using gremlin_eye.Server.Data;
using gremlin_eye.Server.Models;
using gremlin_eye.Server.Repositories;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repostiories
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
            return await _context.GameLogs.Include(gl => gl.Like).Include(gl => gl.Playthroughs).ToListAsync();
        }

        public async Task<List<GameLog>> GetAllLogsByGameId(int gameId)
        {
            return await _context.GameLogs.Include(gl => gl.Like).Include(gl => gl.Playthroughs).ThenInclude(p => p.Review).Where(gl => gl.GameId == gameId).ToListAsync();
        }

        public async Task<GameLog?> GetGameLogByUser(int gameId, int userId)
        {
            return await _context.GameLogs.Include(gl => gl.Like).Include(gl => gl.Playthroughs).ThenInclude(gl => gl.Review).FirstOrDefaultAsync(gl => gl.GameId == gameId && gl.UserId == userId);
        }
    }
}
