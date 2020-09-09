using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class ModelFamilyProfile : Profile
    {
        public ModelFamilyProfile()
        {
            CreateMap<LogicalModel, ModelFamilyDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ModelFamily))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => -1)); // ANTO
            ;
        }
    }
}