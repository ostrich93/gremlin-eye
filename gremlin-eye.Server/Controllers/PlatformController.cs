using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gremlin_eye.Server.Controllers
{
    [Route("api/platform")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlatformController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllPlatforms()
        {
            var platforms = _unitOfWork.Context.Platforms.Select(p => new PlatformDTO
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug
            }).ToArray();

            return Ok(platforms);
        }
    }
}
