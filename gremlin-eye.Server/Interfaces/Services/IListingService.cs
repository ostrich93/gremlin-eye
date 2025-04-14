using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Interfaces.Services
{
    public interface IListingService
    {
        public ICollection<Listing> GetListingWithGame(int gameId);
        public ICollection<Listing> GetAllListings();
    }
}
