using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gremlin_eye.Server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
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
            var (createResult, user) = await _userService.CreateUserAsync(request);
            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors);
            }
            var roleResult = await _userService.AddRoleAsync(user, "User"); //For now, sets users
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            return Ok(createResult);
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

            var user = await _userService.GetUserByName(request.Username); //the service throws an exception if the user isn't found
            if (user == null)
            {
                return Unauthorized($"User with username \"{request.Username}\" was not found");
            }

            //login
            var result = await _userService.Login(request);
            if (result.Succeeded)
            {
                //generate Token
                var roles = await _userService.GetRolesAsync(user);
                string token = _tokenService.GenerateToken(user, roles[0]);
                return Ok(new UserResponseDTO
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Token = token,
                    Role = roles[0]
                });
            }
            return Unauthorized();
        }
    }
}
