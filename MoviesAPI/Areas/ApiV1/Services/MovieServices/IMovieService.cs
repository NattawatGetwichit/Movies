using MoviesAPI.Areas.ApiV1.DTOs;
using MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs;
using MoviesAPI.Areas.ApiV1.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.Services.MovieServices
{
    public interface IMovieService
    {
        Task<ServiceResponse<MovieDtoIndex>> GetAllMovies();

        Task<ServiceResponseWithPagination<List<MovieDto>>> GetAllMoviesPagination(PaginationDto pagination);

        Task<ServiceResponse<MovieDetailDto>> GetMovieById(int id);

        Task<ServiceResponse<MovieDto>> AddMovie(MovieDtoAdd newItem);

        Task<ServiceResponse<MovieDto>> UpdateMovie(int id, MovieDtoUpdate newItem);

        Task<ServiceResponse<MovieDto>> DeleteMovie(int id);

        Task<ServiceResponseWithPagination<List<MovieDto>>> Filter(MovieDtoFilter filter);
    }
}