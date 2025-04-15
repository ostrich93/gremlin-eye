using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IReviewRepository
    {
        Task<int> GetGameReviewCount(long gameId);
    }
}
