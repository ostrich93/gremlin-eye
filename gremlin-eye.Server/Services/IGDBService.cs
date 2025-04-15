using IGDB;
using IGDB.Models;

namespace gremlin_eye.Server.Services
{
    public class IGDBService : IIGDBService
    {
        readonly IGDBClient igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_NAME"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_NAME")
            );

        public async Task<Game[]> GetGames(int offset)
        {
            return await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                $"fields collections.id, collections.name, collections.slug, cover.id," +
                $"first_release_date, genres.id, genres.slug, genres.name, involved_companies.id, involved_companies.name, " +
                $"involved_companies.slug, involved_companies.description, parent_game.id, platforms.id, platforms.name, " +
                $"platforms.slug, summary, slug, screenshots.url; limit 500; offset {offset}; sort id asc;");
        }

        public async Task<Game?> QueryGameAsync(string slug)
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                $"fields collections.id, collections.name, collections.slug, cover.id," +
                $"first_release_date, genres.id, genres.slug, genres.name, involved_companies.id, involved_companies.name, " +
                $"involved_companies.slug, involved_companies.description, parent_game.id, platforms.id, platforms.name, " +
                $"platforms.slug, summary, slug, screenshots.url; where slug = \"{slug}\";");
            return games != null ? games[0] : null;
        }

        public async Task<ICollection<Game>> QuickSearchGamesAsync(string searchItem)
        {
            return await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Search,
                $"""search "{searchItem}"; fields id, name, slug, first_release_date; limit 50;""");
        }
    }
}
