using MoviesAPI.DTOs;
using MoviesAPI.DTOs.GenreDTOs;
using MoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Services
{
    public interface IGenreService
    {
        Task<ServiceResponse<List<GenreDto>>> GetAllGenres();

        Task<ServiceResponse<GenreDto>> GetGenreById(int id);

        Task<ServiceResponse<GenreDto>> AddGenre(GenreDtoAdd newItem);

        Task<ServiceResponse<GenreDto>> UpdateGenre(int id, GenreDtoUpdate newItem);

        Task<ServiceResponse<GenreDto>> DeleteGenre(int id);
    }
}