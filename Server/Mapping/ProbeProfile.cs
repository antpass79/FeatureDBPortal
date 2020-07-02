using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class ProbeProfile : Profile
    {
        public ProbeProfile()
        {
            CreateMap<Probe, ProbeDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SaleName));
            CreateMap<ProbeDTO, Probe>();
        }
    }
}
