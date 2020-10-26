using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Area.ApiV1.DTOs.GenreDTOs
{
    public class GenreDtoUpdate
    {
        [StringLength(40)]
        [FirstLetterUpperCase]
        public string Name { get; set; }
    }
}