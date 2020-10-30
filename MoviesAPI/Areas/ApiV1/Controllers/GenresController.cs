using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesAPI.Area.ApiV1.DTOs.GenreDTOs;
using MoviesAPI.Area.ApiV1.Services.GenreServices;
using System.Threading.Tasks;

namespace MoviesAPI.Area.ApiV1.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly ILogger _logger;

        public GenresController(IGenreService genreService, ILogger logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _genreService.GetAllGenres();

            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "getGenreById")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Find by genre id : {id}");
            var result = await _genreService.GetGenreById(id);

            if (result.IsSuccess == false)
            {
                _logger.LogInformation($"Not found.");
                return NotFound(result);
            }

            _logger.LogInformation($"Found.");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GenreDtoAdd newItem)
        {
            var result = await _genreService.AddGenre(newItem);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GenreDtoUpdate newItem)
        {
            var result = await _genreService.UpdateGenre(id, newItem);

            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _genreService.DeleteGenre(id);

            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}