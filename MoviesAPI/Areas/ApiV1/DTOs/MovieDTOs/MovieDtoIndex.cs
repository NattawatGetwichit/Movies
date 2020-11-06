using MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs
{
    public class MovieDtoIndex
    {
        public List<MovieDto> UpcomingReleases { get; set; }
        public List<MovieDto> InTheaters { get; set; }
    }
}