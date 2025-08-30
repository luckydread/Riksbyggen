using AutoMapper;
using AutoMapper.QueryableExtensions;
using Riksbyggen.Repositories.Interfaces;
using Riksbyggen.Data;
using Microsoft.EntityFrameworkCore;
using Riksbyggen.Dtos;
using Riksbyggen.Models;

namespace Riksbyggen.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CompanyRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyWithApartmentsDto>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                .Include(c => c.Address)
                .Include(c => c.Apartments)
                .ProjectTo<CompanyWithApartmentsDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApartmentDto>> GetApartmentsByCompanyIdAsync(int companyId)
        {
            var apartments = await _context.Apartments
           .Where(a => a.CompanyId == companyId)
           .ProjectTo<ApartmentDto>(_mapper.ConfigurationProvider)
           .ToListAsync();

            return apartments;
        }
        public async Task<CompanyWithApartmentsDto> AddAsync(CompanyCreateDto dto)
        {
            var company = _mapper.Map<Company>(dto);

            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyWithApartmentsDto>(company);
        }

    }
}
