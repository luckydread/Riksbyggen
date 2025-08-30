using Microsoft.AspNetCore.Mvc;
using Riksbyggen.Dtos;
using Riksbyggen.Repositories.Interfaces;

namespace Riksbyggen.Controllers
{
    [ApiController]
    [Route("api/webhooks/apartments")]
    public class ApartmentWebhookController : ControllerBase
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly ILogger<ApartmentWebhookController> _logger;
        private readonly IConfiguration _configuration;

        public ApartmentWebhookController(IApartmentRepository apartmentRepository, ILogger<ApartmentWebhookController> logger, IConfiguration configuration)
        {
            _apartmentRepository = apartmentRepository;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] ApartmentStatusUpdateDto dto)
        {
            // 🔑 Step 1: Validate secret (header or query param)
            var secret = _configuration["Webhook:Secret"];
            var requestSecret = Request.Headers["X-Webhook-Secret"].FirstOrDefault();

            if (requestSecret == null || requestSecret != secret)
            {
                _logger.LogWarning("Unauthorized webhook attempt.");
                return Unauthorized();
            }

            try
            {
                // 🔑 Step 2: Process the update
                var updatedApartment = await _apartmentRepository.UpdateStatusAsync(dto.ApartmentId, dto.Status);

                _logger.LogInformation("Webhook received for Apartment {Id} with Status {Status}", dto.ApartmentId, dto.Status);


                return Ok(new { message = "Status updated successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Apartment with ID {dto.ApartmentId} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing webhook.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}