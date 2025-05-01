using gremlin_eye.Server.Data;
using gremlin_eye.Server.Entity;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<GameData[]> GetGames(int offset, int limit)
        {
            return await _context.Games.Include(g => g.Companies).Include(g => g.Genres).Include(g => g.Platforms).Include(g => g.Series).Skip(offset * limit).OrderBy(g => g.GameId).ToArrayAsync();
        }

        public async Task<GameData?> GetGameById(long id)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.GameId == id);
        }

        public async Task CreateAndSaveAsync(GameData game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task<GameData?> GetGameBySlug(string slug)
        {
            return await _context.Games.Include(g => g.Companies).Include(g => g.Genres).Include(g => g.Series).Include(g => g.Platforms).Where(g => g.Slug == slug).FirstOrDefaultAsync();
        }
    }
}
