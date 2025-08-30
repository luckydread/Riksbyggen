using Microsoft.AspNetCore.Mvc;
using Riksbyggen.Models;
using Riksbyggen.Dtos;
using Riksbyggen.Repositories.Interfaces;

namespace Riksbyggen.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            // Return 201 Created with generated Id
            return CreatedAtAction(nameof(CreateApartment), new { id = apartment.Id }, apartment);
        }

        // 2️⃣ Get all apartments with lease expiring in the next 3 months
        [HttpGet("{companyId}/expiring-leases")]
        public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetExpiringLeases(int companyId)
        {
            var apartments = await _apartmentRepository.GetExpiringLeasesAsync(companyId);
            return Ok(apartments);
        }

        // 3️⃣ Update apartment status
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ApartmentDto>> UpdateStatus(int id, [FromBody] string newStatus)
        {
            var updatedApartment = await _apartmentRepository.UpdateStatusAsync(id, newStatus);
            return Ok(updatedApartment);
        }
    }
}
