using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Riksbyggen.Repositories.Interfaces;
using Riksbyggen.Dtos;
using Riksbyggen.Models;
using Riksbyggen.Data;

namespace Riksbyggen.Repositories
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ApartmentRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApartmentDto> AddAsync(ApartmentCreateDto dto)
        {
            var apartment = _mapper.Map<Apartment>(dto);
            await _context.Apartments.AddAsync(apartment);
            await _context.SaveChangesAsync();

            return _mapper.Map<ApartmentDto>(apartment);
        }

        public async Task<IEnumerable<ApartmentDto>> GetExpiringLeasesAsync(int companyId)
        {
            var threeMonthsFromNow = DateTime.UtcNow.AddMonths(3);

            return await _context.Apartments
                .Where(a => a.CompanyId == companyId && a.LeaseExpiryDate <= threeMonthsFromNow)
                .ProjectTo<ApartmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ApartmentDto> UpdateStatusAsync(int apartmentId, string newStatus)
        {
            var apartment = await _context.Apartments.FindAsync(apartmentId);
            if (apartment == null)
                throw new KeyNotFoundException($"Apartment with ID {apartmentId} not found.");

            apartment.Status = newStatus;
            _context.Apartments.Update(apartment);
            await _context.SaveChangesAsync();

            return _mapper.Map<ApartmentDto>(apartment);
        }

    }

}