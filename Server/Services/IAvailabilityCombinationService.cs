using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface IAvailabilityCombinationService
    {
        public Task<IEnumerable<CombinationDTO>> Get(CombinationSearchDTO search);
    }
}
