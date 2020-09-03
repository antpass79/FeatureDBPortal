using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class DistributorProfile : Profile
    {
        public DistributorProfile()
        {
            CreateMap<Distributor, DistributorDTO>();
            CreateMap<DistributorDTO, Distributor>();
        }
    }
}
