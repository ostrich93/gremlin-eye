using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gremlin_eye.Server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDTO request)
        {
            //Validation

            //create the user
            var userResponse = await _userService.CreateUserAsync(request);
            if (userResponse == null)
            {
                return BadRequest("Bad Request Data");
            }

            return Ok(userResponse);
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            //Validation
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (String.IsNullOrEmpty(request.Username))
                throw new ArgumentException("Username cannot be empty or null", nameof(request.Username));
            if (String.IsNullOrEmpty(request.Password))
                throw new ArgumentException("Password cannot be empty or null", nameof(request.Password));

            //login
            try
            {
                var user = await _userService.LoginAsync(request);
                return Ok(user);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Error logging in: ${ex.Message}");
            }
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userService.LogoutAsync();
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, $"Error logging out: {ex.Message}");
            }
        }
    }
}
