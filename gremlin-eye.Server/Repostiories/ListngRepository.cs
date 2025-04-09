using gremlin_eye.Server.Data;
using gremlin_eye.Server.Models;
using gremlin_eye.Server.Repositories;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repostiories
{
    public class ListingRepository : IListingRepository
    {
        private readonly DataContext _context;
        
        public ListingRepository(DataContext context)
        {
            _context = context;
        }
       
        public async Task<List<Listing>> GetAllListings()
        {
            return await _context.Listings.Include(l => l.ListEntries).ToListAsync();
        }

        public async Task<List<Listing>> GetListingsWithGame(int gameId)
        {
            return await _context.ListEntries.Include(e => e.Listing).Where(e => e.GameId == gameId).Select(e => e.Listing).Distinct().ToListAsync();
        }
    }
}
