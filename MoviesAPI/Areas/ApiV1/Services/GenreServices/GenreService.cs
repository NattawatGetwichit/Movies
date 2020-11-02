using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Area.ApiV1.DTOs.GenreDTOs;
using MoviesAPI.Area.ApiV1.Models;
using MoviesAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Area.ApiV1.Services.GenreServices
{
    public class GenreService : IGenreService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public GenreService(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GenreDto>> AddGenre(GenreDtoAdd newItem)
        {
            var response = new ServiceResponse<GenreDto>();
            Genre genre = _mapper.Map<Genre>(newItem);

            _context.Genres.Add(genre);

            await _context.SaveChangesAsync();

            GenreDto genreDTO = _mapper.Map<GenreDto>(genre);
            response.Data = genreDTO;

            return response;
        }

        public async Task<ServiceResponse<GenreDto>> DeleteGenre(int id)
        {
            var response = new ServiceResponse<GenreDto>();

            Genre genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                response.IsSuccess = false;
                response.Message = $"id = {id} Not found.";

                return response;
            }

            _context.Genres.Remove(genre);

            await _context.SaveChangesAsync();

            GenreDto genreDTO = _mapper.Map<GenreDto>(genre);
            response.Data = genreDTO;

            return response;
        }

        public async Task<ServiceResponse<List<GenreDto>>> GetAllGenres()
        {
            var response = new ServiceResponse<List<GenreDto>>();

            List<Genre> genres = await _context.Genres.AsNoTracking().ToListAsync();

            List<GenreDto> genreDTOs = _mapper.Map<List<GenreDto>>(genres);
            response.Data = genreDTOs;

            return response;
        }

        public async Task<ServiceResponse<GenreDto>> GetGenreById(int id)
        {
            var response = new ServiceResponse<GenreDto>();

            Genre genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                response.IsSuccess = false;
                response.Message = $"id = {id} Not found.";

                return response;
            }

            GenreDto genreDTO = _mapper.Map<GenreDto>(genre);
            response.Data = genreDTO;

            return response;
        }

        public async Task<ServiceResponse<GenreDto>> UpdateGenre(int id, GenreDtoUpdate newItem)
        {
            var response = new ServiceResponse<GenreDto>();

            Genre genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                response.IsSuccess = false;
                response.Message = $"id = {id} Not found.";

                return response;
            }

            genre = _mapper.Map(newItem, genre);

            await _context.SaveChangesAsync();

            GenreDto genreDTO = _mapper.Map<GenreDto>(genre);
            response.Data = genreDTO;

            return response;
        }
    }
}