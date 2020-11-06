using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.DTOs.ActorDTOs
{
    public class ActorDtoUpdate
    {
        public int PersonId { get; set; }
        public string Character { get; set; }
    }
}