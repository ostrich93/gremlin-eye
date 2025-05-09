using gremlin_eye.Server.Repositories;

namespace gremlin_eye.Server.Data
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository Users { get; }
        IGameLogRepository GameLogs { get; }
        IGameRepository Games { get; }
        IPlaythroughRepository Playthroughs { get; }
        IReviewRepository Reviews { get; }
        IListingRepository Lists { get; }
        DataContext Context { get; }
        Task SaveChangesAsync();

    }
}
