using MoviesAPI.DTOs;
using MoviesAPI.DTOs.MovieDTOs;
using MoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Services.MoviesService
{
    public interface IMovieService
    {
        Task<ServiceResponse<List<MovieDto>>> GetAllMovies();

        Task<ServiceResponse<List<MovieDto>>> GetAllMoviesPagination(PaginationDto pagination);

        Task<ServiceResponse<MovieDto>> GetMovieById(int id);

        Task<ServiceResponse<MovieDto>> AddMovie(MovieDtoAdd newItem);

        Task<ServiceResponse<MovieDto>> UpdateMovie(int id, MovieDtoUpdate newItem);

        Task<ServiceResponse<MovieDto>> DeleteMovie(int id);
    }
}