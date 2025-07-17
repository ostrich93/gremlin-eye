using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IReviewRepository
    {
        Task<int> GetGameReviewCount(long gameId);
        Task<List<ReviewDTO>> GetGameTopReviews(long gameId, string slug, string gameName);
        Task<List<ReviewDTO>> GetUserTopReviews(Guid userId);
        Task<ReviewDTO> GetReview(long reviewId);
        Task<List<ReviewDTO>> GetGameReviews(long gameId, string slug, string gameName, int page = 1);
        Task<PaginatedList<ReviewDTO>> GetUserReviews(AppUser user, string orderBy, string sortOrder, int page = 1);
    }
}
