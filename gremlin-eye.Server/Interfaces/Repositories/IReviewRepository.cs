using gremlin_eye.Server.DTOs;

namespace gremlin_eye.Server.Repositories
{
    public interface IReviewRepository
    {
        Task<int> GetGameReviewCount(long gameId);
        Task<List<ReviewDTO>> GetGameTopReviews(long gameId, string slug, string gameName);
        Task<List<ReviewDTO>> GetUserTopReviews(Guid userId);
        Task<ReviewDTO> GetReview(long reviewId);
    }
}
