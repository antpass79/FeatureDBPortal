using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class VersionProfile : Profile
    {
        public VersionProfile()
        {
            CreateMap<MinorVersionAssociation, VersionDTO>();
            CreateMap<VersionDTO, MinorVersionAssociation>();
        }
    }
}
