using MoviesAPI.Areas.ApiV1.DTOs.GenreDTOs;
using MoviesAPI.Areas.ApiV1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.Services.GenreServices
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