using Microsoft.AspNetCore.Mvc;
using Riksbyggen.Dtos;
using Riksbyggen.Repositories.Interfaces;

namespace Riksbyggen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentController : ControllerBase
    {
        private readonly IApartmentRepository _apartmentRepository;

        public ApartmentController(IApartmentRepository apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ApartmentDto>> CreateApartment([FromBody] ApartmentCreateDto dto)
        {
            var apartment = await _apartmentRepository.AddAsync(dto);
            return CreatedAtAction(nameof(CreateApartment), new { id = apartment.Id }, apartment);
        }

        [HttpGet("{companyId}/expiring-leases")]
        public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetExpiringLeases(int companyId)
        {
            var apartments = await _apartmentRepository.GetExpiringLeasesAsync(companyId);
            return Ok(apartments);
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ApartmentDto>> UpdateStatus(int id, [FromBody] string newStatus)
        {
            var updatedApartment = await _apartmentRepository.UpdateStatusAsync(id, newStatus);
            return Ok(updatedApartment);
        }
    }
}
