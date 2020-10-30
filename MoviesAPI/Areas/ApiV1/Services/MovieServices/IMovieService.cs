using MoviesAPI.Area.ApiV1.DTOs;
using MoviesAPI.Area.ApiV1.DTOs.MovieDTOs;
using MoviesAPI.Area.ApiV1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Area.ApiV1.Services.MovieServices
{
    public interface IMovieService
    {
        Task<ServiceResponse<List<MovieDto>>> GetAllMovies();

        Task<ServiceResponseWithPagination<List<MovieDto>>> GetAllMoviesPagination(PaginationDto pagination);

        Task<ServiceResponse<MovieDto>> GetMovieById(int id);

        Task<ServiceResponse<MovieDto>> AddMovie(MovieDtoAdd newItem);

        Task<ServiceResponse<MovieDto>> UpdateMovie(int id, MovieDtoUpdate newItem);

        Task<ServiceResponse<MovieDto>> DeleteMovie(int id);
    }
}