using AutoMapper;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class LayoutTypeProfile : Profile
    {
        public LayoutTypeProfile()
        {
            CreateMap<LayoutType, LayoutTypeDTO>();
            CreateMap<LayoutTypeDTO, LayoutType>();
        }
    }
}
