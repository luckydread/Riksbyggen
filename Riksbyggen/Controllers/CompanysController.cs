using Microsoft.AspNetCore.Mvc;
using Riksbyggen.Dtos;
using Riksbyggen.Repositories.Interfaces;

namespace Riksbyggen.Controllers
{
    [Route("api/[controller]")]
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
            return CreatedAtAction(nameof(GetApartmentsByCompany), new { companyId = createdCompany.Id },createdCompany);
        }


    }
}
