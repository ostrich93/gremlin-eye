using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gremlin_eye.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("/login")]
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

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                
                await _authService.LogoutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error logging out: {ex.Message}");
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
    }
}
