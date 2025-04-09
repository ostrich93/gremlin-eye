using gremlin_eye.Server.Models;

namespace gremlin_eye.Server.Interfaces.Services
{
    public interface IReviewService
    {
        IEnumerable<Review> GetAllReviews();
        Review? GetReviewById(int reviewId);
    }
}
