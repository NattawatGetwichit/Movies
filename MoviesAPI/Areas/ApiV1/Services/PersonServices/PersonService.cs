using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Area.ApiV1.Data;
using MoviesAPI.Area.ApiV1.DTOs;
using MoviesAPI.Area.ApiV1.DTOs.PersonDTOs;
using MoviesAPI.Area.ApiV1.Models;
using MoviesAPI.Area.ApiV1.Services.FileStorageServices;
using MoviesAPI.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Area.ApiV1.Services.PersonServices
{
    public class PersonService : IPersonService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string containerName = "people";

        public PersonService(
            AppDBContext context
            , IMapper mapper
            , IFileStorageService fileStorageService
            , IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _httpContext = httpContext;
        }

        public async Task<ServiceResponse<PersonDto>> AddPerson(PersonDtoAdd newItem)
        {
            var response = new ServiceResponse<PersonDto>();
            Person person = _mapper.Map<Person>(newItem);

            if (newItem.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await newItem.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(newItem.Picture.FileName);

                    person.Picture =
                        await _fileStorageService.SaveFile(content, extension, containerName, newItem.Picture.ContentType);
                }
            }

            _context.People.Add(person);

            await _context.SaveChangesAsync();

            PersonDto personDTO = _mapper.Map<PersonDto>(person);
            response.Data = personDTO;

            return response;
        }

        public async Task<ServiceResponse<PersonDto>> DeletePerson(int id)
        {
            var response = new ServiceResponse<PersonDto>();

            Person person = await _context.People.FindAsync(id);

            if (person == null)
            {
                response.Success = false;
                response.Message = $"id = {id} Not found.";

                return response;
            }

            _context.People.Remove(person);

            await _context.SaveChangesAsync();

            PersonDto personDTO = _mapper.Map<PersonDto>(person);
            response.Data = personDTO;

            return response;
        }

        public async Task<ServiceResponse<List<PersonDto>>> GetAllPeople()
        {
            var response = new ServiceResponse<List<PersonDto>>();

            List<Person> people = await _context.People.AsNoTracking().ToListAsync();

            List<PersonDto> personDTOs = _mapper.Map<List<PersonDto>>(people);
            response.Data = personDTOs;

            return response;
        }

        public async Task<ServiceResponse<List<PersonDto>>> GetAllPeoplePagination(PaginationDto pagination)
        {
            var response = new ServiceResponse<List<PersonDto>>();

            var queryable = _context.People.AsQueryable();
            await _httpContext.HttpContext
                .InsertPaginationParametersInResponse(
                queryable
                , pagination.RecordsPerPage
                , queryable.Count()
                , pagination.Page);
            var people = await queryable.Paginate(pagination).ToListAsync();

            List<PersonDto> personDTOs = _mapper.Map<List<PersonDto>>(people);
            response.Data = personDTOs;

            return response;
        }

        public async Task<ServiceResponse<PersonDto>> GetPersonById(int id)
        {
            var response = new ServiceResponse<PersonDto>();

            Person person = await _context.People.FindAsync(id);

            if (person == null)
            {
                response.Success = false;
                response.Message = $"id = {id} Not found.";

                return response;
            }

            PersonDto personDTO = _mapper.Map<PersonDto>(person);
            response.Data = personDTO;

            return response;
        }

        public async Task<ServiceResponse<PersonDto>> UpdatePerson(int id, PersonDtoUpdate newItem)
        {
            var response = new ServiceResponse<PersonDto>();

            Person person = await _context.People.FindAsync(id);

            if (person == null)
            {
                response.Success = false;
                response.Message = $"id = {id} Not found.";
            }

            person = _mapper.Map(newItem, person);

            if (newItem.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await newItem.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(newItem.Picture.FileName);

                    person.Picture =
                        await _fileStorageService.EditFile(content, extension, containerName, person.Picture, newItem.Picture.ContentType);
                }
            }

            _context.People.Update(person);

            await _context.SaveChangesAsync();

            PersonDto PersonDTO = _mapper.Map<PersonDto>(person);
            response.Data = PersonDTO;

            return response;
        }
    }
}