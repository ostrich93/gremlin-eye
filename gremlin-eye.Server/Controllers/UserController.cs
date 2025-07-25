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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
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

        [HttpGet("header/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserHeader(string username)
        {
            var user = await _userService.GetUserByName(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserHeaderResponse
            {
                UserName = username,
                Id = user.Id,
                AvatarUrl = user.AvatarUrl
            });
        }

        [HttpGet("profile/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(string username)
        {
            try
            {
                var userProfile = await _userService.GetUserProile(username);
                return Ok(userProfile);
            } catch(Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
