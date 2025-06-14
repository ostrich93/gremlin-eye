﻿using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using LinqKit;

namespace gremlin_eye.Server.Repositories
{
    public interface IGameRepository
    {
        Task<GameData?> GetGameBySlug(string slug);
        Task<GameData[]> GetGames(int offset, int limit);
        Task<GameData?> GetGameById(long id);
        Task CreateAndSaveAsync(GameData game);
        void Create(GameData data);
        Task CreateRangeAndSaveAsync(IEnumerable<GameData> data);
        Task UpdateRangeAndSaveAsync(IEnumerable<GameData> data);
        Task<GameData[]> SearchGames(string query);
        Task<List<GameData>?> GetRelatedGames(long seriesId, long gameId);
        Task<PaginatedList<GameSummaryDTO>> GetPaginatedList(ExpressionStarter<GameData> predicate, string orderBy, string sortOrder, int page = 1);
    }
}
