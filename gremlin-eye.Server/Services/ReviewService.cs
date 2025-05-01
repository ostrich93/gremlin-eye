using gremlin_eye.Server.Data;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public class ReviewService : IReviewService
    {
        private UnitOfWork _unitOfWork;
        
        public ReviewService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /* public ICollection<Review> GetAllReviews()
        {
            return _unitOfWork.Reviews.ToList();
        }

        public Review? GetReviewById(int reviewId)
        {
            return _context.Reviews.First(r => r.ReviewId == reviewId);
        }
        */
    }
}
