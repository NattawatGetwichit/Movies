using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTOs;
using MoviesAPI.DTOs.PersonDTOs;
using MoviesAPI.Services;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _PersonService;

        public PeopleController(IPersonService PersonService)
        {
            _PersonService = PersonService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _PersonService.GetAllPeople();

            return Ok(result);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationDto pagination)
        {
            var result = await _PersonService.GetAllPeoplePagination(pagination);

            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "getPersonById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _PersonService.GetPersonById(id);

            if (result.Success == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] PersonDtoAdd newItem)
        {
            var result = await _PersonService.AddPerson(newItem);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] PersonDtoUpdate newItem)
        {
            var result = await _PersonService.UpdatePerson(id, newItem);

            if (result.Success == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _PersonService.DeletePerson(id);

            if (result.Success == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}