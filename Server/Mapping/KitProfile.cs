using AutoMapper;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class KitProfile : Profile
    {
        public KitProfile()
        {
            CreateMap<BiopsyKits, KitDTO>();
            CreateMap<KitDTO, BiopsyKits>();
        }
    }
}
