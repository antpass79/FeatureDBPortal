using AutoMapper;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDTO>();
            CreateMap<CountryDTO, Country>();
        }
    }
}
