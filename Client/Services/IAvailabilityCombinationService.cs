using FeatureDBPortal.Shared;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface IAvailabilityCombinationService
    {
        Task<CombinationDTO> GetCombination(CombinationSearchDTO search);
    }
}
