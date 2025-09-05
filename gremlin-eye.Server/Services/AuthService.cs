using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace gremlin_eye.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public AuthService(IConfiguration configuration, IPasswordHasher passwordHasher, ITokenService tokenService, IUnitOfWork unitOfWork, IMailService mailService)
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public async Task GenerateValidationToken(string emailAddress)
        {
            if (!Regex.IsMatch(emailAddress, "^([a-z0-9_.-]+)@([\\da-z.-]+).([a-z.]{2,6})$"))
                throw new ArgumentException("Email address is invalid");

            AppUser? user = _unitOfWork.Users.GetUserByEmail(emailAddress);
            if (user == null)
            {
                throw new ArgumentException("Email not found");
            }

            string token = _tokenService.GeneratePasswordVerificationToken(user);
            
            user.ConfirmationToken = token;
            await _unitOfWork.SaveChangesAsync();

            string messageBody = $"<span>Dear {user.UserName},<br><br>You have requested to reset your password.<br><br>To set a new password, follow this link: {_configuration["DomainName"]}/resetPassword/{token} <br><br>Have a great day!";
            MailData mailData = new MailData
            {
                MailToName = user.UserName,
                MailToId = user.Email,
                MailSubject = "Password Reset",
                MailBody = messageBody
            };

            bool mailSent = _mailService.SendMail(mailData);
            if (!mailSent)
            {
                throw new Exception("Confirmation Email could not be sent");
            }
        }

        public async Task HandlePasswordChange(PasswordChangeRequest changeRequest)
        {
            if (changeRequest.UserId.HasValue)
            {
                try
                {
                    await HandleLoggedInPasswordChange(changeRequest);
                }
                catch (Exception _e)
                {
                    throw;
                }
            }

            else
            {
                try
                {
                    await HandleLoggedOutPasswordChange(changeRequest);
                }
                catch (Exception _e)
                {
                    throw;
                }
            }
        }

        private async Task HandleLoggedInPasswordChange(PasswordChangeRequest changeRequest)
        {
            var user = _unitOfWork.Users.GetUserById(changeRequest.UserId.Value);

            if (user == null)
            {
                throw new Exception("Error. The user could not be found in the database");
            }

            if (string.IsNullOrWhiteSpace(changeRequest.Password))
                throw new Exception("Error. You cannot set the password as an empty string");
            if (string.IsNullOrWhiteSpace(changeRequest.PasswordConfirmation))
                throw new Exception("Error. The password confirmation cannot be empty.");
            if (!Regex.IsMatch(changeRequest.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                throw new Exception("New password is invalid. It must be an alphanumeric string with at least 8 characters");
            if (!changeRequest.PasswordConfirmation.Equals(changeRequest.Password))
                throw new Exception("The password and confirmation input do not match");

            _passwordHasher.HashPassword(changeRequest.Password, out string hashedPassword, out byte[] salt);
            user.PasswordHash = hashedPassword;
            user.Salt = salt;

            user.ConfirmationToken = string.Empty;
            //generate new stamp and set here
            user.Stamp = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

            _unitOfWork.Context.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task HandleLoggedOutPasswordChange(PasswordChangeRequest changeRequest)
        {
            if (string.IsNullOrWhiteSpace(changeRequest.ValidationToken))
            {
                throw new Exception("Error: No password validation token");
            }

            if (!_tokenService.CheckResetTokenExpiration(changeRequest.ValidationToken))
                throw new Exception("The password validation token has expired");

            var user = _unitOfWork.Users.GetUserByConfirmationToken(changeRequest.ValidationToken);

            if (user == null)
                throw new Exception("Error: User with matching token could not be found");

            if (string.IsNullOrWhiteSpace(changeRequest.Password))
                throw new Exception("Error. You cannot set the password as an empty string");
            if (string.IsNullOrWhiteSpace(changeRequest.PasswordConfirmation))
                throw new Exception("Error. The password confirmation cannot be empty.");
            if (!Regex.IsMatch(changeRequest.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                throw new Exception("New password is invalid. It must be an alphanumeric string with at least 8 characters");
            if (!changeRequest.PasswordConfirmation.Equals(changeRequest.Password))
                throw new Exception("The password and confirmation input do not match");

            _passwordHasher.HashPassword(changeRequest.Password, out string hashedPassword, out byte[] salt);
            user.PasswordHash = hashedPassword;
            user.Salt = salt;

            user.ConfirmationToken = string.Empty;
            //generate new stamp and set here
            user.Stamp = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

            _unitOfWork.Context.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserResponseDTO> LoginAsync(LoginDTO request)
        {
            var foundUser = await _unitOfWork.Users.GetUserWithTokensAsync(request.Username);

            if (foundUser == null)
            {
                throw new Exception($"User {request.Username} was not found");
            }

            bool verified = _passwordHasher.VerifyPassword(request.Password, foundUser.PasswordHash, foundUser.Salt);
            if (verified)
            {
                var token = _tokenService.GenerateAccessToken(foundUser);
                var refreshToken = _tokenService.GenerateRefreshToken();
                foundUser.RefreshTokens.RemoveAll(t => !t.IsActive); //remove expired tokens
                foundUser.RefreshTokens.Add(new RefreshToken
                {
                    User = foundUser,
                    UserId = foundUser.Id,
                    Token = refreshToken,
                    ExpiresIn = DateTime.UtcNow.AddDays(7)
                });
                await _unitOfWork.Context.SaveChangesAsync();
                return new UserResponseDTO
                {
                    UserId = foundUser.Id,
                    Username = foundUser.UserName,
                    Email = foundUser.Email,
                    Role = foundUser.Role,
                    AccessToken = token,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                throw new Exception("The password was invalid");
            }
        }

        public async Task LogoutAsync(Guid userId, string refreshToken)
        {
            var user = _unitOfWork.Users.GetUserById(userId);

            if (user == null)
            {
                throw new Exception("Invalid logout request: User with id was not found");
            }

            var userToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (userToken != null && userToken.IsActive)
            {
                userToken.Revoked = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        public TokenDTO RefreshToken(TokenDTO request)
        {
            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;

            var principal = _tokenService.GetPrincipalFromToken(accessToken);
            var userId = Guid.Parse(principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var user = _unitOfWork.Users.GetUserById(userId);

            if (user == null)
            {
                throw new Exception("Invalid Token Request: User not found");
            }

            var userToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (userToken == null || !userToken.IsActive)
            {
                throw new Exception("Invalid Token Request: No active refresh token found");
            }

            //if (user == null || user.RefreshToken ==

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            return new TokenDTO { AccessToken = newAccessToken, RefreshToken = refreshToken };

        }

        public bool ValidatePasswordVerificationToken(string code)
        {
            AppUser? user = _unitOfWork.Users.GetUserByConfirmationToken(code);
            if (user == null)
                return false;

            return _tokenService.ValidatePasswordVerificationToken(code, user);

        }
    }
}
