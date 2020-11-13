using System;

namespace MoviesAPI.Areas.ApiV1.DTOs.IdentityDTOs
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}