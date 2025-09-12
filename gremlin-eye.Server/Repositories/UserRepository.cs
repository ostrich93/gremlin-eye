using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public AppUser? GetUserByName(string username)
        {
            return _context.Users.Include(u => u.RefreshTokens).FirstOrDefault(u => u.UserName == username);
        }

        public async Task<AppUser?> GetUserByNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public AppUser? GetUserById(Guid userId)
        {
            return _context.Users.Include(u => u.RefreshTokens).FirstOrDefault(u => u.Id == userId);
            
        }

        public Task<AppUser?> GetUserWithTokensAsync(string username)
        {
            return _context.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.UserName == username);
        }

        public UserProfileResponse GetUserProfile(AppUser user)
        {
            var totalGamesPlayed = _context.GameLogs.AsNoTracking().Where(g => g.UserId == user.Id).Count(g => g.IsPlayed);
            var totalGamesBacklogged = _context.GameLogs.AsNoTracking().Where(g => g.UserId == user.Id).Count(g => g.IsBacklog);
            var gamesPlayedThisYear = _context.GameLogs.AsNoTracking().Where(g => g.UserId == user.Id).Count(g => g.IsPlayed && g.UpdatedAt.Year == DateTime.UtcNow.Year);

            var ratingCandidates = _context.GameLogs.Where(g => g.UserId == user.Id).SelectMany(g => g.Playthroughs);
            int[] trueRatingCounts = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            RatingCount[] ratings = ratingCandidates.Where(p => p.Rating > 0)
                .GroupBy(p => p.Rating).Select(g =>
                    new RatingCount
                    {
                        Rating = g.Key,
                        Count = g.Count()
                    }).ToArray();


            if (ratings.Any())
            {
                foreach(RatingCount rc in ratings)
                {
                    trueRatingCounts[rc.Rating-1] = rc.Count;
                }
            }

            return new UserProfileResponse
            {
                GamesBacklogged = totalGamesBacklogged,
                GamesPlayedThisYear = gamesPlayedThisYear,
                TotalGamesPlayed = totalGamesPlayed,
                Username = user.UserName,
                RatingCounts = trueRatingCounts
            };
        }

        public AppUser? SearchUser(RegisterUserRequestDTO registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Username) || string.IsNullOrEmpty(registerRequest.Email))
                return null;
            return _context.Users.Where(u => registerRequest.Username == u.UserName || registerRequest.Email == u.Email).FirstOrDefault();
        }

        public AppUser? GetUserByEmail(string email)
        {
            //vaildation on email string handled by service calling this function
            return _context.Users.Where(u => email == u.Email).FirstOrDefault();
        }

        public AppUser? GetUserByConfirmationToken(string token)
        {
            return _context.Users.Where(u => token.Equals(u.ConfirmationToken)).FirstOrDefault();
        }
    }
}
