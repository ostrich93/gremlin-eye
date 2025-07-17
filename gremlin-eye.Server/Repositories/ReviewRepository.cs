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

        public async Task<List<ReviewDTO>> GetGameReviews(long gameId, string slug, string gameName, int page = 1)
        {
            var playthroughs = await _context.Playthroughs.Include(p => p.Platform).Include(p => p.Review).Include(p => p.GameLog).ThenInclude(g => g.User).Where(p => p.GameId == gameId && p.Review != null && !string.IsNullOrEmpty(p.ReviewText)).OrderByDescending(p => p.CreatedAt).Skip(32 * (page -1)).Take(32).ToListAsync();
            List<ReviewDTO> reviews = new List<ReviewDTO>();
            foreach (Playthrough playthrough in playthroughs)
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
            var playthrough = await _context.Playthroughs.Include(p => p.Game).Include(p => p.Platform).Include(p => p.GameLog).Where(p => p.Id == reviewData.PlaythroughId).FirstAsync();

            List<CommentDTO> responseComments = new List<CommentDTO>();

            if (comments.Count > 0)
            {
                responseComments = comments.Select(c => new CommentDTO
                {
                    CommentId = c.Id,
                    Username = c.Author.UserName,
                    AvatarUrl = c.Author.AvatarUrl,
                    CommentBody = c.CommentBody,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).OrderBy(c => c.CreatedAt).ToList();
            }

            return new ReviewDTO
            {
                ReviewId = reviewId,
                GameId = playthrough.GameId,
                PlaythroughId = playthrough.Id,
                UserId = reviewData.User.Id,
                GameName = playthrough.Game.Name,
                Username = reviewData.User.UserName,
                AvatarUrl = reviewData.User.AvatarUrl,
                GameSlug = playthrough.Game.Slug,
                CoverUrl = playthrough.Game.CoverUrl,
                PlayStatus = playthrough.GameLog.PlayStatus ?? PlayState.Played,
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
                Comments = responseComments
            };
        }

        public async Task<PaginatedList<ReviewDTO>> GetUserReviews(AppUser user, string orderBy, string sortOrder, int page = 1)
        {
            var totalItems = await _context.Reviews.AsNoTracking().Where(r => r.UserId == user.Id).CountAsync();
            List<Playthrough> playthroughs;

            if (sortOrder == Constants.DESC)
                playthroughs = await _context.Playthroughs.Include(p => p.GameLog).Include(p => p.Game).Include(p => p.Platform).Where(p => p.GameLog.UserId == user.Id && p.Review != null).Include(p => p.Review).ThenInclude(r => r.Comments).OrderByDescending(p => p.CreatedAt).Skip(15 * (page - 1)).Take(15).ToListAsync();
            else
                playthroughs = await _context.Playthroughs.Include(p => p.GameLog).Include(p => p.Game).Include(p => p.Platform).Where(p => p.GameLog.UserId == user.Id && p.Review != null).Include(p => p.Review).ThenInclude(r => r.Comments).OrderBy(p => p.CreatedAt).Skip(15 * (page - 1)).Take(15).ToListAsync();

            PaginatedList<ReviewDTO> paginatedList = new PaginatedList<ReviewDTO>
            {
                Items = playthroughs.Select(p => new ReviewDTO
                {
                    GameId = p.Game.Id,
                    GameName = p.Game.Name,
                    GameSlug = p.Game.Slug,
                    CoverUrl = p.Game.CoverUrl,
                    ReleaseDate = p.Game.ReleaseDate,
                    UserId = user.Id,
                    Username = user.UserName,
                    PlayStatus = p.GameLog.PlayStatus ?? PlayState.Played,
                    Platform = p.Platform != null ? new PlatformDTO
                    {
                        Id = p.Platform.Id,
                        Name = p.Platform.Name,
                        Slug = p.Platform.Slug
                    } : null,
                    ReviewId = p.Review.Id,
                    ReviewText = p.ReviewText,
                    ContainsSpoilers = p.ReviewSpoilers,
                    Rating = p.Rating > 0 ? p.Rating : null,
                    CommentCount = p.Review.Comments.Count
                }).ToList(),
                TotalItems = totalItems,
                PageNumber = page,
                PageLimit = 15
            };
            return paginatedList;
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
