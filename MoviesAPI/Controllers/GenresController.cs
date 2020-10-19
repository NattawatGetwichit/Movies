using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesAPI.DTOs;
using MoviesAPI.Models;
using MoviesAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
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
            var result = await _genreService.GetGenreById(id);

            if (result.Success == false)
            {
                return NotFound(result);
            }

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

            if (result.Success == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _genreService.DeleteGenre(id);

            if (result.Success == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}