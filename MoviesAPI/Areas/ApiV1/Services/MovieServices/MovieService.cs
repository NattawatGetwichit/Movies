using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Area.ApiV1.Data;
using MoviesAPI.Area.ApiV1.DTOs;
using MoviesAPI.Area.ApiV1.DTOs.MovieDTOs;
using MoviesAPI.Area.ApiV1.Models;
using MoviesAPI.Area.ApiV1.Services.FileStorageServices;
using MoviesAPI.Data;
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
            Movie movie = _mapper.Map<Movie>(newItem);

            //return ResponseResult.Success(new MovieDto());

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

            return ResponseResult.Success(movieDTO);
        }

        public async Task<ServiceResponse<MovieDto>> DeleteMovie(int id)
        {
            Movie movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return ResponseResult.Failure<MovieDto>($"id = {id} Not found.");
            }

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            MovieDto movieDTO = _mapper.Map<MovieDto>(movie);

            return ResponseResult.Success(movieDTO);
        }

        public async Task<ServiceResponse<List<MovieDto>>> GetAllMovies()
        {
            List<Movie> movies = await _context.Movies.AsNoTracking().ToListAsync();

            List<MovieDto> movieDTOs = _mapper.Map<List<MovieDto>>(movies);

            return ResponseResult.Success(movieDTOs);
        }

        public async Task<ServiceResponseWithPagination<List<MovieDto>>> GetAllMoviesPagination(PaginationDto pagination)
        {
            var queryable = _context.Movies.AsQueryable();
            var paginationResult = await _httpContext.HttpContext
                .InsertPaginationParametersInResponse(
                queryable
                , pagination.RecordsPerPage
                , pagination.Page);
            var movies = await queryable.Paginate(pagination).ToListAsync();

            List<MovieDto> movieDTOs = _mapper.Map<List<MovieDto>>(movies);

            return ResponseResultWithPagination.Success(movieDTOs, paginationResult);
        }

        public async Task<ServiceResponse<MovieDto>> GetMovieById(int id)
        {
            Movie movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return ResponseResult.Failure<MovieDto>($"id = {id} Not found.");
            }

            MovieDto movieDTO = _mapper.Map<MovieDto>(movie);

            return ResponseResult.Success(movieDTO);
        }

        public async Task<ServiceResponse<MovieDto>> UpdateMovie(int id, MovieDtoUpdate newItem)
        {
            Movie movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return ResponseResult.Failure<MovieDto>($"id = {id} Not found.");
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

            var ma = _context.MoviesActors.Where(x => x.MovieId == movie.Id).ToList();

            _context.MoviesActors.RemoveRange(ma);

            var mg = _context.MoviesGenres.Where(x => x.MovieId == movie.Id).ToList();

            _context.MoviesGenres.RemoveRange(mg);

            //await _context.Database
            //    .ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id};");

            AnnotateActorsOrder(movie);

            _context.Movies.Update(movie);

            await _context.SaveChangesAsync();

            MovieDto movieDTO = _mapper.Map<MovieDto>(movie);

            return ResponseResult.Success(movieDTO);
        }

        private static void AnnotateActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }
    }
}