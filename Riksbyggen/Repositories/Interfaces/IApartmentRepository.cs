using Riksbyggen.Dtos;
namespace Riksbyggen.Repositories.Interfaces
{
    public interface IApartmentRepository
    {
        Task<ApartmentDto> AddAsync(ApartmentCreateDto dto);
        Task<IEnumerable<ApartmentDto>> GetExpiringLeasesAsync(int companyId);

        Task<ApartmentDto> UpdateStatusAsync(int apartmentId, string newStatus);
    }
}