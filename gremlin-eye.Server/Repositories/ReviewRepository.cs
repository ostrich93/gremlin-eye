using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Enums;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<int> GetGameReviewCount(long gameId)
        {
            return await _context.Playthroughs.Where(p => p.GameId == gameId && !string.IsNullOrWhiteSpace(p.ReviewText)).CountAsync();
            //return await _context.GameLogs.Where(g => g.GameId == gameId).Include(g => g.Playthroughs.Where(p => !string.IsNullOrWhiteSpace(p.ReviewText))).CountAsync();
        }

        //Get the top 5 reviews for a GamePage
        public async Task<List<ReviewDTO>> GetGameTopReviews(long gameId, string slug, string gameName)
        {
            var playthroughs = await _context.Playthroughs.Include(p => p.Platform).Include(p => p.Review).Include(p => p.GameLog).ThenInclude(g => g.User).Where(p => p.GameId == gameId && p.Review != null && !string.IsNullOrEmpty(p.ReviewText)).OrderByDescending(p => p.CreatedAt).Take(5).ToListAsync();
            List<ReviewDTO> reviews = new List<ReviewDTO>();
            foreach(Playthrough playthrough in playthroughs)
            {
                reviews.Add(new ReviewDTO
                {
                    ReviewId = playthrough.Review.Id,
                    GameId = playthrough.GameId,
                    PlaythroughId = playthrough.Id,
                    UserId = playthrough.GameLog.UserId,
                    GameName = gameName,
                    AvatarUrl = playthrough.GameLog.User.AvatarUrl ?? string.Empty,
                    Username = playthrough.GameLog.User.UserName,
                    GameSlug = slug,
                    ReviewText = playthrough.ReviewText,
                    ContainsSpoilers = playthrough.ReviewSpoilers,
                    Rating = playthrough.Rating > 0 ? playthrough.Rating : null,
                    Platform = playthrough.Platform != null ? new PlatformDTO
                    {
                        Id = playthrough.Platform.Id,
                        Name = playthrough.Platform.Name,
                        Slug = playthrough.Platform.Slug
                    } : null,
                    CommentCount = playthrough.Review.Comments.Count,
                    PlayStatus = playthrough.GameLog.PlayStatus ?? PlayState.Played
                });
            }

            return reviews;
        }

        //used for ReviewPage
        public async Task<ReviewDTO> GetReview(long reviewId)
        {
            var reviewData = await _context.Reviews.Include(r => r.User).Where(r => r.Id == reviewId).FirstAsync();
            var comments = await _context.ReviewComments.Include(c => c.Author).Where(c => c.ReviewId == reviewId).ToListAsync();
            var playthrough = await _context.Playthroughs.Include(p => p.Game).Include(p => p.Platform).Include(p => p.GameLog).Where(p => p.Id == reviewId).FirstAsync();

            return new ReviewDTO
            {
                ReviewId = reviewId,
                GameId = reviewData.Playthrough.GameId,
                PlaythroughId = playthrough.Id,
                UserId = reviewData.User.Id,
                GameName = playthrough.Game.Name,
                Username = reviewData.User.UserName,
                AvatarUrl = reviewData.User.AvatarUrl,
                GameSlug = playthrough.Game.Slug,
                ReviewText = playthrough.ReviewText,
                ContainsSpoilers = playthrough.ReviewSpoilers,
                Rating = playthrough.Rating > 0 ? playthrough.Rating : null,
                Platform = playthrough.Platform != null ? new PlatformDTO
                {
                    Id = playthrough.Platform.Id,
                    Name = playthrough.Platform.Name,
                    Slug = playthrough.Platform.Slug
                } : null,
                CommentCount = comments.Count,
                Comments = comments.Select(c => new CommentDTO
                {
                    CommentId = c.Id,
                    Username = c.Author.UserName,
                    AvatarUrl = c.Author.AvatarUrl,
                    CommentBody = c.CommentBody,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).OrderBy(c => c.CreatedAt).ToList()
            };
        }

        //Get Top 5 Reviews for a user in UserProfile page
        public async Task<List<ReviewDTO>> GetUserTopReviews(Guid userId)
        {
            var playthroughs = await _context.Playthroughs.Include(p => p.Review).Include(p => p.Platform).Include(p => p.Game).Include(p => p.GameLog).ThenInclude(g => g.User).Where(p => p.Review != null && p.GameLog.UserId == userId).OrderByDescending(p => p.CreatedAt).Take(5).ToListAsync();
            List<ReviewDTO> reviews = new List<ReviewDTO>();
            foreach(Playthrough playthrough in playthroughs)
            {
                reviews.Add(new ReviewDTO
                {
                    ReviewId = playthrough.Review.Id,
                    GameId = playthrough.GameId,
                    PlaythroughId = playthrough.Id,
                    UserId = userId,
                    Username = playthrough.GameLog.User.UserName,
                    GameName = playthrough.Game.Name,
                    GameSlug = playthrough.Game.Slug,
                    CoverUrl = playthrough.Game.CoverUrl,
                    ContainsSpoilers = playthrough.ReviewSpoilers,
                    Rating = playthrough.Rating > 0 ? playthrough.Rating : null,
                    Platform = playthrough.Platform != null ? new PlatformDTO
                    {
                        Id = playthrough.Platform.Id,
                        Name = playthrough.Platform.Name,
                        Slug = playthrough.Platform.Slug
                    } : null,
                    ReviewText = playthrough.ReviewText,
                    PlayStatus = playthrough.GameLog.PlayStatus ?? PlayState.Played,
                    CommentCount = playthrough.Review.Comments.Count
                });
            }
            return reviews;
        }
    }
}
