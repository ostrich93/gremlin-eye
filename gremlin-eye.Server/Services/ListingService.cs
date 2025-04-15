using gremlin_eye.Server.Data;
using gremlin_eye.Server.Interfaces.Services;
using gremlin_eye.Server.Entity;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Services
{
    public class ListingService : IListingService
    {
        private UnitOfWork _unitOfWork;

        public ListingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<Listing>> GetAllListings()
        {
            return await _unitOfWork.Lists.GetAllListings();
        }

        public async Task<ICollection<Listing>> GetListingWithGame(int gameId)
        {
            List<Listing> listings = await _unitOfWork.Lists.GetListingsWithGame(gameId);
            List<Listing> filteredList = new List<Listing>();
            if (listings == null || listings.Count == 0)
            {
                return filteredList;
            }

            foreach (Listing l in listings)
            {
                if (l.ListEntries.Any(e => e.GameId == gameId))
                {
                    filteredList.Add(l);
                }
            }
            return filteredList;
        }
    }
}
