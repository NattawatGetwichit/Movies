using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}