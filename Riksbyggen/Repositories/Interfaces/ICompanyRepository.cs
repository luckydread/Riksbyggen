using Riksbyggen.Dtos;

namespace Riksbyggen.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
         Task<CompanyWithApartmentsDto> AddAsync(CompanyCreateDto dto);
        Task<IEnumerable<CompanyWithApartmentsDto>> GetAllCompaniesAsync();

        Task<IEnumerable<ApartmentDto>> GetApartmentsByCompanyIdAsync(int companyId);
    }
}
