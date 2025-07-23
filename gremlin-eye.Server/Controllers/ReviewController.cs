using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace gremlin_eye.Server.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{reviewId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGameReview(long reviewId)
        {
            try
            {
                var review = await _unitOfWork.Reviews.GetReview(reviewId);
                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("addComment")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SubmitComment(CommentRequest comment)
        {
            Claim idClaim = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(idClaim!.Value);

            var user = _unitOfWork.Users.GetUserById(userId);
            if (user == null)
                return Unauthorized("User not found");

            var review = await _unitOfWork.Context.Reviews.Where(r => r.Id == comment.ReviewId).FirstAsync();
            ReviewComment newComment = new ReviewComment
            {
                Author = user,
                AuthorId = userId,
                Review = review,
                ReviewId = review.Id,
                CommentBody = comment.CommentBody,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            review.Comments.Add(newComment);

            await _unitOfWork.SaveChangesAsync();
            return Ok(new CommentDTO
            {
                CommentId = newComment.Id,
                Username = user.UserName,
                AvatarUrl = user.AvatarUrl ?? "",
                CommentBody = newComment.CommentBody,
                CreatedAt = newComment.CreatedAt,
                UpdatedAt = newComment.UpdatedAt
            });
        }

        [HttpPatch("comment/{commentId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> EditComment(long commentId, [FromBody] string commentBody)
        {
            Claim idClaim = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(idClaim!.Value);

            var user = _unitOfWork.Users.GetUserById(userId);
            if (user == null)
                return Unauthorized("User not found");

            var comment = await _unitOfWork.Context.ReviewComments.Where(c => c.Id == commentId).FirstAsync();
            comment.CommentBody = commentBody;
            comment.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return Ok(new CommentDTO
            {
                CommentId = comment.Id,
                Username = user.UserName,
                AvatarUrl = user.AvatarUrl ?? "",
                CommentBody = comment.CommentBody,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            });
        }

        [HttpGet("users/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserReviews(string username, [FromQuery] string orderBy = Constants.ORDER_RECENT, [FromQuery] string sortOrder = Constants.DESC, [FromQuery] int page = 1)
        {
            var user = _unitOfWork.Users.GetUserByName(username);
            if (user == null)
                return NotFound("Could not find user");

            var reviews = await _unitOfWork.Reviews.GetUserReviews(user, orderBy, sortOrder, page);

            return Ok(reviews);
        }
    }
}
