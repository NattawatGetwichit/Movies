using Microsoft.AspNetCore.Http;
using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoviesAPI.Validations.ContentTypeValidator;

namespace MoviesAPI.DTOs.MovieDTOs
{
    public class MovieDtoUpdate
    {
        [Required(ErrorMessage = "The field with name {0} is required.")]
        [StringLength(40)]
        [FirstLetterUpperCase]
        public string Title { get; set; }

        public string Summary { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }

        [FileSizeValidator(4)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }
    }
}