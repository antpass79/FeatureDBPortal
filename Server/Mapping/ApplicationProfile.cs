using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Application, ApplicationDTO>();
            CreateMap<ApplicationDTO, Application>();
        }
    }
}
