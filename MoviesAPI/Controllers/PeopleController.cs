using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTOs;
using MoviesAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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