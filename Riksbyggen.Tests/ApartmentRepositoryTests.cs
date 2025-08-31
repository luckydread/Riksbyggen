using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Riksbyggen.Data;
using Riksbyggen.Repositories;
using Riksbyggen.Models;
using Riksbyggen.Dtos;


namespace Riksbyggen.Tests
{
    public class ApartmentRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IMapper _mapper;

        public ApartmentRepositoryTests()
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
        public async Task AddAsync_ShouldAddApartmentToDatabase()
        {
            var context = CreateContext();
            var repository = new ApartmentRepository(context, _mapper);

            // Arrange

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

            var result = context.Apartments.SingleOrDefault(x => x.CompanyId == apartment.CompanyId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetExpiringLeasesAsync_ShouldReturnOnlyExpiringApartments()
        {
            var context = CreateContext();
            var repository = new ApartmentRepository(context, _mapper);

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
                LeaseExpiryDate = DateTime.UtcNow.AddMonths(1),
                CompanyId = company.Id,
                Status = "Available"
            };

            var apartment = _mapper.Map<Apartment>(apartmentDto);
            await context.Apartments.AddAsync(apartment);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetExpiringLeasesAsync(company.Id);

            // Assert
            Assert.Single(result);
            Assert.Equal(apartment.Id, result.First().Id);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateApartmentStatus()
        {
            var context = CreateContext();
            var repository = new ApartmentRepository(context, _mapper);


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
                LeaseExpiryDate = DateTime.UtcNow.AddMonths(1),
                CompanyId = company.Id,
                Status = "Available"
            };

            var apartment = _mapper.Map<Apartment>(apartmentDto);
            await context.Apartments.AddAsync(apartment);
            await context.SaveChangesAsync();

            // Act
            var newStatus = "Busy";
            var updated = await repository.UpdateStatusAsync(apartment.Id, newStatus);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(newStatus, updated.Status);

        }

    }


}
