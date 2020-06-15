using FeatureDBPortal.Shared;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface IAvailabilityCombinationService
    {
        public Task<CombinationDTO> Get(CombinationSearchDTO search);
    }
}
