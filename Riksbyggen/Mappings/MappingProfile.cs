using AutoMapper;
using Riksbyggen.Models;
using Riksbyggen.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyWithApartmentsDto>()
        .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
        .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
        .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
        .ForMember(dest => dest.Apartments, opt => opt.MapFrom(src => src.Apartments));

        CreateMap<Apartment, ApartmentDto>()
        .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Adress.Street))
        .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Adress.City))
        .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Adress.ZipCode));

        CreateMap<CompanyCreateDto, Company>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                Street = src.Street,
                City = src.City,
                ZipCode = src.ZipCode
            }));

        CreateMap<ApartmentCreateDto, Apartment>()
        .ForMember(dest => dest.Adress, opt => opt.MapFrom(src => new Address
        {
            Street = src.Street,
            City = src.City,
            ZipCode = src.ZipCode
        }));
    }
}
