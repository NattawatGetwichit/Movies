using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Areas.ApiV1.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field with name {0} is required.")]
        [StringLength(40)]
        [FirstLetterUpperCase]
        public string Name { get; set; }

        public string Biography { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Picture { get; set; }

        public List<MoviesActors> MoviesActors { get; set; }
    }
}