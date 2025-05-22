using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public interface IGameSyncService
    {
        Task ImportGames(int page = 1);
        Task ImportGenres();
        Task ImportPlatforms();
        Task ImportSeries(int page = 1);
        Task ImportCompanies(int page = 1);
    }
}
