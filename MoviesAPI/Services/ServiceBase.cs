using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Services
{
    public class ServiceBase
    {
        protected readonly IHttpContextAccessor _httpContext;

        public ServiceBase(IHttpContextAccessor httpContext)
        {
            ResetNow();
            _httpContext = httpContext;
        }

        public Func<DateTime> Now { get; private set; } = () => DateTime.Now;

        public void SetNow(DateTime now) => Now = () => now;

        public void ResetNow() => Now = () => DateTime.Now;

        public string GetUsername() => _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        public IEnumerable<string> GetUserRoles()
        {
            return _httpContext.HttpContext.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        }

        //public async Task SetCurrentUser(Guid userId, string userName, IEnumerable<string> roles)
        //{
        //    //TODO:
        //    // query user

        //    var user = await Users.FindAsync(userId);

        //    // if not found: create new user.
        //    if (user == null)
        //    {
        //        user = new User
        //        {
        //            Id = userId,
        //            UserName = userName,
        //            CreatedDate = Now(),
        //            LastSignInDate = Now(),
        //            Note = string.Empty
        //        };

        //        Users.Add(user);
        //        await SaveChangesAsync();
        //    }

        //    // if found: add additional data from IdentityUser into User.
        //    user.Roles = roles;

        //    // set current user
        //    CurrentUser = user;
        //}
    }
}