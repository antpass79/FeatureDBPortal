using AutoMapper;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Name));
            CreateMap<CountryDTO, Country>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CountryName));
        }
    }
}
