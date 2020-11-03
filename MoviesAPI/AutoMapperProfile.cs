using AutoMapper;
using MoviesAPI.Area.ApiV1.DTOs.GenreDTOs;
using MoviesAPI.Area.ApiV1.DTOs.MovieDTOs;
using MoviesAPI.Area.ApiV1.DTOs.PersonDTOs;
using MoviesAPI.Area.ApiV1.Models;
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
        }

        private List<MoviesGenres> MapMoviesGenresAdd(MovieDtoAdd movieDto, Movie movie)
        {
            var result = new List<MoviesGenres>();
            foreach (var id in movieDto.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
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
            foreach (var id in movieDto.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }
            return result;
        }

        private List<MoviesActors> MapMoviesActorsUpdate(MovieDtoUpdate movieDto, Movie movie)
        {
            var result = new List<MoviesActors>();
            foreach (var actor in movieDto.Actors)
            {
                result.Add(new MoviesActors() { PersonId = actor.PersonId, Character = actor.Character });
            }
            return result;
        }
    }
}