using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI.Areas.ApiV1.DTOs;
using MoviesAPI.Areas.ApiV1.DTOs.IdentityDTOs;
using MoviesAPI.Data;
using MoviesAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class AccountsControllers : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public AccountsControllers(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            AppDBContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new IdentityUser { UserName = model.EmailAddress, Email = model.EmailAddress };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.EmailAddress,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }

        [HttpPost("RenewToken")]
        [Authorize]
        public ActionResult<UserToken> Renew()
        {
            var userInfo = new UserInfo
            {
                EmailAddress = HttpContext.User.Identity.Name
            };

            var result = BuildToken(userInfo);

            return Ok(result);
        }

        private async Task<UserToken> BuildToken(UserInfo userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.EmailAddress),
                new Claim(ClaimTypes.Email, userInfo.EmailAddress),
                new Claim("mykey","Whatever I want")
            };

            var identityUser = await _userManager.FindByEmailAsync(userInfo.EmailAddress);

            var userRoles = await _userManager.GetRolesAsync(identityUser);

            foreach (var item in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] PaginationDto paginationDTO)
        {
            var queryable = _context.Users.AsQueryable();
            queryable = queryable.OrderBy(x => x.Email);
            await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordsPerPage, paginationDTO.Page);
            var users = await queryable.Paginate(paginationDTO).ToListAsync();

            var resultToReturn = _mapper.Map<List<UserDto>>(users);

            return Ok(resultToReturn);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<string>>> GetRoles()
        {
            var roles = await _context.Roles.Select(x => x.Name).ToListAsync();
            return Ok(roles);
        }

        [HttpPost("AssignUserRole")]
        public async Task<ActionResult> AssignRole(UserRoleDtoUpdate user_DTO_ToEditRole)
        {
            var user = await _userManager.FindByIdAsync(user_DTO_ToEditRole.UserId);
            if (user == null)
            {
                return NotFound("user not found");
            }

            var role = await _roleManager.FindByNameAsync(user_DTO_ToEditRole.RoleName);
            if (role == null)
            {
                return NotFound("role not found");
            }

            await _userManager.AddToRoleAsync(user, user_DTO_ToEditRole.RoleName);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, user_DTO_ToEditRole.RoleName));

            return Ok();
        }

        [HttpPost("RemoveUserRole")]
        public async Task<ActionResult> RemoveRole(UserRoleDtoUpdate user_DTO_ToEditRole)
        {
            var user = await _userManager.FindByIdAsync(user_DTO_ToEditRole.UserId);
            if (user == null)
            {
                return NotFound("user not found");
            }

            var role = await _roleManager.FindByNameAsync(user_DTO_ToEditRole.RoleName);
            if (role == null)
            {
                return NotFound("role not found");
            }

            await _userManager.RemoveFromRoleAsync(user, user_DTO_ToEditRole.RoleName);
            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, user_DTO_ToEditRole.RoleName));

            return Ok();
        }
    }
}