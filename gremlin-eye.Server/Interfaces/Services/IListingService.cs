using gremlin_eye.Server.Models;

namespace gremlin_eye.Server.Interfaces.Services
{
    public interface IListingService
    {
        IEnumerable<Listing> GetListingWithGame(int gameId);
        IEnumerable<Listing> GetAllListings();
    }
}
