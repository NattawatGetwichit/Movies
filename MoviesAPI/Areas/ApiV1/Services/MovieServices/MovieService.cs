using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Area.ApiV1.Data;
using MoviesAPI.Area.ApiV1.DTOs;
using MoviesAPI.Area.ApiV1.DTOs.MovieDTOs;
using MoviesAPI.Area.ApiV1.Models;
using MoviesAPI.Area.ApiV1.Services.FileStorageServices;
using MoviesAPI.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Area.ApiV1.Services.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string containerName = "movies";

        public MovieService(
            AppDBContext context
            , IMapper mapper
            , IFileStorageService fileStorageService
            , IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _httpContext = httpContext;
        }

        public async Task<ServiceResponse<MovieDto>> AddMovie(MovieDtoAdd newItem)
        {
            var response = new ServiceResponse<MovieDto>();
            Movie movie = _mapper.Map<Movie>(newItem);

            //return response;

            if (newItem.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await newItem.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(newItem.Poster.FileName);

                    movie.Poster =
                        await _fileStorageService.SaveFile(content, extension, containerName, newItem.Poster.ContentType);
                }
            }

            AnnotateActorsOrder(movie);

            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();

            MovieDto movieDTO = _mapper.Map<MovieDto>(movie);
            response.Data = movieDTO;

            return response;
        }

        public async Task<ServiceResponse<MovieDto>> DeleteMovie(int id)
        {
            var response = new ServiceResponse<MovieDto>();

            Movie movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                response.Success = false;
                response.Message = $"id = {id} Not found.";

                return response;
            }

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            MovieDto personDTO = _mapper.Map<MovieDto>(movie);
            response.Data = personDTO;

            return response;
        }

        public async Task<ServiceResponse<List<MovieDto>>> GetAllMovies()
        {
            var response = new ServiceResponse<List<MovieDto>>();

            List<Movie> movies = await _context.Movies.AsNoTracking().ToListAsync();

            List<MovieDto> movieDTOs = _mapper.Map<List<MovieDto>>(movies);
            response.Data = movieDTOs;

            return response;
        }

        public async Task<ServiceResponse<List<MovieDto>>> GetAllMoviesPagination(PaginationDto pagination)
        {
            var response = new ServiceResponse<List<MovieDto>>();

            var queryable = _context.Movies.AsQueryable();
            await _httpContext.HttpContext
                .InsertPaginationParametersInResponse(
                queryable
                , pagination.RecordsPerPage
                , queryable.Count()
                , pagination.Page);
            var movies = await queryable.Paginate(pagination).ToListAsync();

            List<MovieDto> movieDTOs = _mapper.Map<List<MovieDto>>(movies);
            response.Data = movieDTOs;

            return response;
        }

        public async Task<ServiceResponse<MovieDto>> GetMovieById(int id)
        {
            var response = new ServiceResponse<MovieDto>();

            Movie movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                response.Success = false;
                response.Message = $"id = {id} Not found.";

                return response;
            }

            MovieDto movieDTO = _mapper.Map<MovieDto>(movie);
            response.Data = movieDTO;

            return response;
        }

        public async Task<ServiceResponse<MovieDto>> UpdateMovie(int id, MovieDtoUpdate newItem)
        {
            var response = new ServiceResponse<MovieDto>();

            Movie movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                response.Success = false;
                response.Message = $"id = {id} Not found.";
            }

            movie = _mapper.Map(newItem, movie);

            if (newItem.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await newItem.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(newItem.Poster.FileName);

                    movie.Poster =
                        await _fileStorageService.EditFile(content, extension, containerName, movie.Poster, newItem.Poster.ContentType);
                }
            }

            _context.Movies.Update(movie);

            await _context.SaveChangesAsync();

            MovieDto movieDTO = _mapper.Map<MovieDto>(movie);
            response.Data = movieDTO;

            return response;
        }

        private static void AnnotateActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = 1;
                }
            }
        }
    }
}