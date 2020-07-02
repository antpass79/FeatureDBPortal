using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;
using GrpcCombination;

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

    public class GRPCLayoutTypeProfile : Profile
    {
        public GRPCLayoutTypeProfile()
        {
            CreateMap<LayoutType, LayoutTypeGRPC>()
                .ConvertUsing(value => (LayoutTypeGRPC)(value + 1));

            CreateMap<LayoutTypeGRPC, LayoutType>()
                .ConvertUsing(value => (LayoutType)(value - 1));
        }
    }
}
