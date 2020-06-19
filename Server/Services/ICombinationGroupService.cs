using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface ICombinationGroupService
    {
        Task<CombinationDTO> Combine(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy);
    }
}
