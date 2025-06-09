using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gremlin_eye.Server.Controllers
{
    [Route("api/genre")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllGenres()
        {
            var genres = _unitOfWork.Context.Genres.Select(p => new GenreDTO
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug
            }).ToArray();

            return Ok(genres);
        }
    }
}
