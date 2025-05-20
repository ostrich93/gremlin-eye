using gremlin_eye.Server.Data;

namespace gremlin_eye.Server.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public int GetGameLikeCount(long gameId)
        {
            return _context.GameLikes.Count(g => g.GameId == gameId);
        }
    }
}
