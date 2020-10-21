using MoviesAPI.DTOs;
using MoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Services
{
    public interface IPersonService
    {
        Task<ServiceResponse<List<PersonDto>>> GetAllPeople();

        Task<ServiceResponse<PersonDto>> GetPersonById(int id);

        Task<ServiceResponse<PersonDto>> AddPerson(PersonDtoAdd newItem);

        Task<ServiceResponse<PersonDto>> UpdatePerson(int id, PersonDtoUpdate newItem);

        Task<ServiceResponse<PersonDto>> DeletePerson(int id);
    }
}