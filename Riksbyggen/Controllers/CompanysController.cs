using Microsoft.AspNetCore.Mvc;
using Riksbyggen.Dtos;
using Riksbyggen.Repositories.Interfaces;

namespace Riksbyggen.Controllers
{
    [Route("api/Companies")]
    [ApiController]

    public class CompanysController : ControllerBase
    {
        private readonly ICompanyRepository _repository;

        public CompanysController(ICompanyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyWithApartmentsDto>>> GetAllCompanies()
        {
            var companies = await _repository.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{companyId}/apartments")]
        public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetApartmentsByCompany(int companyId)
        {
            var apartments = await _repository.GetApartmentsByCompanyIdAsync(companyId);
            return Ok(apartments);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyWithApartmentsDto>> CreateCompany([FromBody] CompanyCreateDto dto)
        {
            var createdCompany = await _repository.AddAsync(dto);

            // Return 201 Created with link to company's apartments
            return CreatedAtAction(nameof(GetApartmentsByCompany),
                                   new { companyId = createdCompany.Id },
                                   createdCompany);
        }


    }
}
