using AutoMapper;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Mapping
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<LogicalModel, ModelDTO>();
            CreateMap<ModelDTO, LogicalModel>();
        }
    }
}
