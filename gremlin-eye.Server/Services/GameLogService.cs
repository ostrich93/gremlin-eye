using gremlin_eye.Server.Data;
using gremlin_eye.Server.Interfaces.Services;
using gremlin_eye.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Services
{
    public class GameLogService : IGameLogService
    {
        private readonly DataContext _context;
        
        public GameLogService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<GameLog> GetAllGameLogs()
        {
            return _context.GameLogs.ToList();
        }

        public IEnumerable<GameLog> GetAllLogsByGameId(int gameId)
        {
            return _context.GameLogs.Where(gl => gl.GameId == gameId).ToList();
        }

        public IEnumerable<GameLog> GetBackloggedByGame(int gameId)
        {
            return _context.GameLogs.Where(gl => gl.GameId == gameId && gl.IsBacklog).ToList();
        }

        public IEnumerable<GameLog> GetBackloggedByUser(int userId)
        {
            return _context.GameLogs.Include(gl => gl.User).Where(gl => gl.User.UserId == userId && gl.IsBacklog).ToList();
        }

        public GameLog? GetGameLogByUser(int gameId, int userId)
        {
            return _context.GameLogs.Include(l => l.User).Where(l => l.User.UserId == userId && l.GameId == gameId).FirstOrDefault();
        }

        public IEnumerable<GameLog> GetPlayedLogsByGame(int gameId)
        {
            return _context.GameLogs.Where(gl => gl.GameId == gameId && gl.PlayStatus != null).ToList();
        }

        public IEnumerable<GameLog> GetPlayedLogsByUser(int userId)
        {
            return _context.GameLogs.Include(gl => gl.User).Where(gl => gl.User.UserId == userId && gl.PlayStatus != null).ToList();
        }

        public IEnumerable<GameLog> GetPlayingLogsByUser(int userId)
        {
            return _context.GameLogs.Include(gl => gl.User).Where(gl => gl.User.UserId == userId && gl.IsPlaying).ToList();
        }

        public IEnumerable<GameLog> GetPlayingLogsByGame(int gameId)
        {
            return _context.GameLogs.Where(gl => gl.GameId == gameId && gl.IsPlaying).ToList();
        }

        public IEnumerable<GameLog> GetWishlistByGame(int gameId)
        {
            return _context.GameLogs.Where(gl => gl.GameId == gameId && gl.IsWishlist).ToList();
        }

        public IEnumerable<GameLog> GetWishlistByUser(int userId)
        {
            return _context.GameLogs.Include(gl => gl.User).Where(gl => gl.User.UserId == userId && gl.IsWishlist).ToList();
        }
    }
}
