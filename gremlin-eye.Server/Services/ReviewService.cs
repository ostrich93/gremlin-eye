using gremlin_eye.Server.Data;
using gremlin_eye.Server.Interfaces.Services;
using gremlin_eye.Server.Models;

namespace gremlin_eye.Server.Services
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _context;
        
        public ReviewService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Review> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review? GetReviewById(int reviewId)
        {
            return _context.Reviews.First(r => r.ReviewId == reviewId);
        }
    }
}
