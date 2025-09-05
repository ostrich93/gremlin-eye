using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gremlin_eye.Server.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            //Validation
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Username))
                throw new ArgumentException("Username cannot be empty or null", nameof(request.Username));
            if (string.IsNullOrEmpty(request.Password))
                throw new ArgumentException("Password cannot be empty or null", nameof(request.Password));

            //login
            try
            {
                var user = await _authService.LoginAsync(request);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error logging in: ${ex.Message}");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(TokenDTO logoutRequest)
        {
            try
            {
                if (logoutRequest is null || string.IsNullOrWhiteSpace(logoutRequest.RefreshToken))
                {
                    return BadRequest("Error logging out: no refresh token was passed in.");
                }

                Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                Guid? userId = idClaim != null ? Guid.Parse(idClaim!.Value) : null;

                if (userId != null)
                    await _authService.LogoutAsync(userId.Value, logoutRequest.RefreshToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error logging out: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenDTO tokenRequest)
        {
            if (tokenRequest is null || string.IsNullOrWhiteSpace(tokenRequest.AccessToken) || string.IsNullOrWhiteSpace(tokenRequest.RefreshToken))
            {
                return BadRequest("Invalid client request");
            }
            
            try
            {
                var refreshTokenResponse = _authService.RefreshToken(tokenRequest);
                return Ok(refreshTokenResponse);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                return BadRequest("Email Address cannot be empty");

            try
            {
                await _authService.GenerateValidationToken(emailAddress);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordChangeRequest changeRequest)
        {
            try
            {
                await _authService.HandlePasswordChange(changeRequest);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("canResetPassword")]
        public IActionResult ValidatePasswordReset([FromQuery] string token)
        {
            try
            {
                bool isTokenValid = _authService.ValidatePasswordVerificationToken(token);
                return Ok();
            } catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
