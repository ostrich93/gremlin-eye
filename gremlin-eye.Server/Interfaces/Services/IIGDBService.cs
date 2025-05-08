using IGDB.Models;

namespace gremlin_eye.Server.Services
{
    public interface IIGDBService
    {
        Task<Game[]> GetGames(int offset);
        Task<Game?> QueryGameAsync(string slug);
        Task<ICollection<Game>> QuickSearchGamesAsync(string searchItem);
        Task<Genre[]> GetGenres();
        Task<Collection[]> GetSeries(int offset);
        Task<Platform[]> GetPlatforms();
        Task<Company[]> GetCompanies(int offset);
    }
}
