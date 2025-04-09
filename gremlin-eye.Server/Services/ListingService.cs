using gremlin_eye.Server.Data;
using gremlin_eye.Server.Interfaces.Services;
using gremlin_eye.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Services
{
    public class ListingService : IListingService
    {
        private readonly DataContext _context;

        public ListingService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Listing> GetAllListings()
        {
            return _context.Listings.ToList();
        }

        public IEnumerable<Listing> GetListingWithGame(int gameId)
        {
            List<Listing> listings = _context.Listings.Include(l => l.ListEntries).ToList();
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
