using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.DTOs.IdentityDTOs
{
    public class UserDto
    {
        public string EmailAddress { get; set; }
        public string UserId { get; set; }
    }
}