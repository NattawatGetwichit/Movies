using MoviesAPI.Areas.ApiV1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs
{
    public class MovieDtoFilter : PaginationDto
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool UpcomingReleases { get; set; }
        public bool InTheaters { get; set; }
    }
}