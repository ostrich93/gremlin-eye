using gremlin_eye.Server.Entity;
using IGDB.Models;

namespace gremlin_eye.Server.Repositories
{
    public interface IGameRepository
    {
        public Task<GameData?> GetGameBySlug(string slug);
    }
}
