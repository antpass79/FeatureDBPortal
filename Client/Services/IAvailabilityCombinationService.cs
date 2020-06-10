using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface IAvailabilityCombinationService
    {
        Task<IEnumerable<CombinationDTO>> GetCombinations(CombinationSearchDTO search);
    }
}
