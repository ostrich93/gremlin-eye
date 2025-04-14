using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IListingRepository
    {
        public Task<List<Listing>> GetAllListings();
        public Task<List<Listing>> GetListingsWithGame(int gameId);
    }
}
