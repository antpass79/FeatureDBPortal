using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class CertifierProfile : Profile
    {
        public CertifierProfile()
        {
            CreateMap<Certifier, CertifierDTO>();
            CreateMap<CertifierDTO, Certifier>();
        }
    }
}