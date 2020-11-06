using MoviesAPI.Areas.ApiV1.DTOs;
using MoviesAPI.Areas.ApiV1.DTOs.PersonDTOs;
using MoviesAPI.Areas.ApiV1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.Services.PersonServices
{
    public interface IPersonService
    {
        Task<ServiceResponse<List<PersonDto>>> GetAllPeople();

        Task<ServiceResponse<List<PersonDto>>> GetAllPeoplePagination(PaginationDto pagination);

        Task<ServiceResponse<PersonDto>> GetPersonById(int id);

        Task<ServiceResponse<PersonDto>> AddPerson(PersonDtoAdd newItem);

        Task<ServiceResponse<PersonDto>> UpdatePerson(int id, PersonDtoUpdate newItem);

        Task<ServiceResponse<PersonDto>> DeletePerson(int id);
    }
}