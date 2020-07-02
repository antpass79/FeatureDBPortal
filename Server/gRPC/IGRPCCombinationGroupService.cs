using FeatureDBPortal.Server.Data.Models.RD;
using GrpcCombination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.gRPC
{
    public interface IGRPCCombinationGroupService
    {
        Task<CombinationGRPC> Combine(CombinationSearchGRPC search, IEnumerable<LayoutType> groupBy);
    }
}
