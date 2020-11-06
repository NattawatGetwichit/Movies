using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Areas.ApiV1.DTOs.ActorDTOs;
using MoviesAPI.Helpers;
using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MoviesAPI.Validations.ContentTypeValidator;

namespace MoviesAPI.Areas.ApiV1.DTOs.MovieDTOs
{
    public class MovieDtoAdd
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

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorDtoAdd>>))]
        public List<ActorDtoAdd> Actors { get; set; }
    }
}