using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Areas.ApiV1.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field with name {0} is required.")]
        [StringLength(40)]
        [FirstLetterUpperCase]
        public string Title { get; set; }

        public string Summary { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }

        public string Poster { get; set; }

        public List<MoviesActors> MoviesActors { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
    }
}