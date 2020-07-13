using FeatureDBPortal.Shared;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface ICombinationGroupService
    {
        Task<CombinationDTO> GetCombination(CombinationSearchDTO search);
    }
}
