using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.DTOs.IdentityDTOs
{
    public class UserRoleDtoUpdate
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}