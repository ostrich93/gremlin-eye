using IGDB.Models;

namespace gremlin_eye.Server.Services
{
    public interface IIGDBService
    {
        public Task<Game?> QueryGameAsync(string slug);
        public Task<ICollection<Game>> QuickSearchGamesAsync(string searchItem);
    }
}
