using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MoviesAPI.Areas.ApiV1.DTOs.ActorDTOs;
using MoviesAPI.Areas.ApiV1.DTOs.GenreDTOs;
using MoviesAPI.Areas.ApiV1.DTOs.IdentityDTOs;
using MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs;
using MoviesAPI.Areas.ApiV1.DTOs.PersonDTOs;
using MoviesAPI.Areas.ApiV1.Models;
using System.Collections.Generic;

namespace MoviesAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<GenreDtoAdd, Genre>();
            CreateMap<GenreDtoUpdate, Genre>();

            CreateMap<Person, PersonDto>().ReverseMap();
            CreateMap<PersonDtoAdd, Person>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<PersonDtoUpdate, Person>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<MovieDtoAdd, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenresAdd))
            .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActorsAdd));

            CreateMap<MovieDtoUpdate, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenresUpdate))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActorsUpdate));

            CreateMap<Movie, MovieDetailDto>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.Actors, options => options.MapFrom(MapMoviesActors));

            CreateMap<IdentityUser, UserDto>()
                .ForMember(x => x.EmailAddress, options => options.MapFrom(x => x.Email))
                .ForMember(x => x.UserId, options => options.MapFrom(x => x.Id));
        }

        private List<GenreDto> MapMoviesGenres(Movie movieDto, MovieDetailDto movieDetailDto)
        {
            var result = new List<GenreDto>();
            foreach (var moviesGenres in movieDto.MoviesGenres)
            {
                result.Add(new GenreDto() { Id = moviesGenres.GenreId, Name = moviesGenres.Genre.Name });
            }
            return result;
        }

        private List<ActorDto> MapMoviesActors(Movie movieDto, MovieDetailDto movieDetailDto)
        {
            var result = new List<ActorDto>();
            foreach (var moviesActors in movieDto.MoviesActors)
            {
                result.Add(new ActorDto()
                {
                    PersonId = moviesActors.PersonId,
                    PersonName = moviesActors.Person.Name,
                    Character = moviesActors.Character
                });
            }
            return result;
        }

        private List<MoviesGenres> MapMoviesGenresAdd(MovieDtoAdd movieDto, Movie movie)
        {
            var result = new List<MoviesGenres>();
            foreach (var genreId in movieDto.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = genreId });
            }
            return result;
        }

        private List<MoviesActors> MapMoviesActorsAdd(MovieDtoAdd movieDto, Movie movie)
        {
            var result = new List<MoviesActors>();
            foreach (var actor in movieDto.Actors)
            {
                result.Add(new MoviesActors() { PersonId = actor.PersonId, Character = actor.Character });
            }
            return result;
        }

        private List<MoviesGenres> MapMoviesGenresUpdate(MovieDtoUpdate movieDto, Movie movie)
        {
            var result = new List<MoviesGenres>();
            foreach (var genreId in movieDto.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = genreId });
            }
            return result;
        }

        private List<MoviesActors> MapMoviesActorsUpdate(MovieDtoUpdate movieDto, Movie movie)
        {
            var result = new List<MoviesActors>();
            foreach (var actor in movieDto.Actors)
            {
                result.Add(new MoviesActors()
                {
                    PersonId = actor.PersonId,
                    Character = actor.Character
                });
            }
            return result;
        }
    }
}