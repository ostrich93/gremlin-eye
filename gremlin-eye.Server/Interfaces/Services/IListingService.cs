using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Interfaces.Services
{
    public interface IListingService
    {
        Task<ICollection<Listing>> GetListingWithGame(int gameId);
        Task<ICollection<Listing>> GetAllListings();
    }
}
