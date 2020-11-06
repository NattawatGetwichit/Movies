using MoviesAPI.Areas.ApiV1.DTOs.ActorDTOs;
using MoviesAPI.Areas.ApiV1.DTOs.GenreDTOs;
using System.Collections.Generic;

namespace MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs
{
    public class MovieDetailDto : MovieDto
    {
        public List<GenreDto> Genres { get; set; }

        public List<ActorDto> Actors { get; set; }
    }
}