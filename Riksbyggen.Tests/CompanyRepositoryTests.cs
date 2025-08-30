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
        public async Task AddAsync_ShouldThrowValidationException_IfDtoIsInvalid()
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



        /*
                [Fact]
                public async Task GetByIdAsync_ShouldReturnCompany_IfCompanyExists()
                {
                    var context = CreateContext();
                    var repository = new CompanyRepository(context);
                    var company = new Company
                    {
                        Name = "TestCompany",
                        Address = new Address
                        {
                            Street = "123 Test St",
                            City = "Test City",
                            ZipCode = "12345"
                        }
                    };

                    await context.Companies.AddAsync(company);
                    await context.SaveChangesAsync();

                    var result = await repository.GetByIdAsync(company.Id);

                    Assert.NotNull(result);
                    Assert.Equal("TestCompany", result.Name);
                }

                [Fact]
                public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_IfCompanyDoesNotExist()
                {
                    // Arrange
                    var context = CreateContext();
                    var repository = new CompanyRepository(context);

                    await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetByIdAsync(5000));
                }

                [Fact]
                public async Task GetAllAsync_ShouldReturnAllCompanies()
                {
                    // Arrange
                    var context = CreateContext();
                    var repository = new CompanyRepository(context);
                    var company1 = new Company
                    {
                        Name = "TestCompany1",
                        Address = new Address
                        {
                            Street = "123 Test St",
                            City = "Test City",
                            ZipCode = "12345"
                        }
                    };
                    var company2 = new Company
                    {
                        Name = "TestCompany2",
                        Address = new Address
                        {
                            Street = "456 Test St",
                            City = "Test City",
                            ZipCode = "12345"
                        }
                    };
                    await context.Companies.AddRangeAsync(company1, company2);
                    await context.SaveChangesAsync();

                    var result = await repository.GetAllAsync();

                    Assert.NotNull(result);
                    Assert.True(result.Count() >= 2);
                }

                [Fact]
                public async Task UpdateAsync_ShouldUpdateCompanyInDatabase()
                {
                    var context = CreateContext();
                    var repository = new CompanyRepository(context);
                    var company = new Company
                    {
                        Name = "Test Company",
                        Address = new Address
                        {
                            Street = "123 Test St",
                            City = "Test City",
                            ZipCode = "12345"
                        }
                    };
                    await context.Companies.AddAsync(company);
                    await context.SaveChangesAsync();

                    company.Name = "Updated Company";

                    await repository.UpdateAsync(company);

                    var result = context.Companies.Find(company.Id);

                    Assert.NotNull(result);
                    Assert.Equal("Updated Company", result.Name);
                }*/
    }
}