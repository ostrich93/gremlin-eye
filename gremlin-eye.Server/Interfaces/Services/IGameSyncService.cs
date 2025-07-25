using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public interface IGameSyncService
    {
        Task<long> ImportGames(int page = 1);
        Task<long> ImportGenres();
        Task<long> ImportPlatforms();
        Task<long> ImportSeries(int page = 1);
        Task<long> ImportCompanies(int page = 1);
    }
}
