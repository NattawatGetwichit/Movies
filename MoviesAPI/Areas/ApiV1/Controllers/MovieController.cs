using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Areas.ApiV1.DTOs;
using MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs;
using MoviesAPI.Areas.ApiV1.Services.MovieServices;

using MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs;

using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService MovieService)
        {
            _movieService = MovieService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _movieService.GetAllMovies();

            return Ok(result);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationDto pagination)
        {
            var result = await _movieService.GetAllMoviesPagination(pagination);

            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "getMovieById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _movieService.GetMovieById(id);

            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MovieDtoAdd newItem)
        {
            var result = await _movieService.AddMovie(newItem);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MovieDtoUpdate newItem)
        {
            var result = await _movieService.UpdateMovie(id, newItem);

            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _movieService.DeleteMovie(id);

            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilter([FromQuery] MovieDtoFilter filter)
        {
            var result = await _movieService.Filter(filter);

            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}