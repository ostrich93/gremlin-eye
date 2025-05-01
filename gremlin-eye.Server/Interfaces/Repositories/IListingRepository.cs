using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IListingRepository
    {
        Task<List<Listing>> GetAllListings();
        Task<List<Listing>> GetListingsWithGame(int gameId);
    }
}
