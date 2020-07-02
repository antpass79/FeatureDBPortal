using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class OptionProfile : Profile
    {
        public OptionProfile()
        {
            CreateMap<Option, OptionDTO>();
            CreateMap<OptionDTO, Option>();
        }
    }
}
