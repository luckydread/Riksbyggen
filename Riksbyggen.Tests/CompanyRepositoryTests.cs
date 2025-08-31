using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Riksbyggen.Data;
using Riksbyggen.Repositories;
using Riksbyggen.Models;
using Riksbyggen.Dtos;

namespace Riksbyggen.Tests
{
    public class CompanyRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IMapper _mapper;

        public CompanyRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var config = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<CompanyCreateDto, Company>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                Street = src.Street,
                City = src.City,
                ZipCode = src.ZipCode
            }));

        cfg.CreateMap<Company, CompanyWithApartmentsDto>()
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ForMember(dest => dest.Apartments, opt => opt.MapFrom(src => src.Apartments));

        cfg.CreateMap<ApartmentCreateDto, Apartment>()
            .ForMember(dest => dest.Adress, opt => opt.MapFrom(src => new Address
            {
                Street = src.Street,
                City = src.City,
                ZipCode = src.ZipCode
            }));

        cfg.CreateMap<Apartment, ApartmentDto>()
        .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Adress.Street))
        .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Adress.City))
        .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Adress.ZipCode));
    });
            _mapper = config.CreateMapper();
        }


        private ApplicationDbContext CreateContext() => new ApplicationDbContext(_dbContextOptions);

        [Fact]
        public async Task AddAsync_ShouldAddCompanyToDatabase()
        {
            var context = CreateContext();
            var repository = new CompanyRepository(context, _mapper);

            var dto = new CompanyCreateDto
            {
                Name = "Test Company",
                Street = "123 Test St",
                City = "Test City",
                ZipCode = "12345",
            };

            await repository.AddAsync(dto);

            var result = context.Companies.SingleOrDefault(x => x.Name == "Test Company");

            Assert.NotNull(result);
            Assert.Equal("Test Company", result.Name);
        }
        [Fact]
        public async Task GetApartmentsByCompanyIdAsync_ShouldReturnApartments_WhenApartmentsExist()
        {
            // Arrange
            var context = CreateContext();
            var repository = new CompanyRepository(context, _mapper);

            var companyDto = new CompanyCreateDto
            {
                Name = "Company With Apartments",
                Street = "Street 2",
                City = "City",
                ZipCode = "54321"
            };

            var company = _mapper.Map<Company>(companyDto);
            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var apartmentDto = new ApartmentCreateDto
            {
                Street = "Apt Street",
                City = "City",
                ZipCode = "54321",
                NumberOfRooms = 2,
                LeaseExpiryDate = DateTime.UtcNow.AddMonths(6),
                CompanyId = company.Id,
                Status = "Available"
            };

            var apartment = _mapper.Map<Apartment>(apartmentDto);
            await context.Apartments.AddAsync(apartment);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetApartmentsByCompanyIdAsync(company.Id);

            // Assert
            var list = result.ToList();
            Assert.NotNull(list);
            Assert.Single(list);
            Assert.Equal(apartmentDto.Street, list[0].Street);
            Assert.Equal(apartmentDto.NumberOfRooms, list[0].NumberOfRooms);
            Assert.Equal(company.Id, list[0].CompanyId);
        }

        [Fact]
        public async Task GetAllCompaniesAsync_ShouldReturnAllCompanies()
        {
            // Arrange
            var context = CreateContext();
            var repository = new CompanyRepository(context, _mapper);

            var dto = new CompanyCreateDto
            {
                Name = "Company 1",
                Street = "123 Test St",
                City = "Test City",
                ZipCode = "12345",
            };

            var company = _mapper.Map<Company>(dto);

            var dto2 = new CompanyCreateDto
            {
                Name = "Company 2",
                Street = "123 Test St",
                City = "Test City",
                ZipCode = "12345",
            };

            var company2 = _mapper.Map<Company>(dto2);

            await context.Companies.AddRangeAsync(company, company2);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllCompaniesAsync();

            // Assert
            var list = result.ToList();
            Assert.NotNull(list);
            Assert.Contains(list, x => x.Name == "Company 1");
            Assert.Contains(list, x => x.Name == "Company 2");
        }

    }
}