using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.RuleManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services.RuleManagement
{
    public interface IRequirementRuleService
    {
        Task<IEnumerable<OptionDTO>> GetOptionsAsync();
        Task<IEnumerable<ApplicationDTO>> GetApplicationsAsync();
        Task<IEnumerable<ProbeDTO>> GetProbesAsync();
        Task<IEnumerable<KitDTO>> GetKitsAsync();
        Task<IEnumerable<ModelDTO>> GetModelsAsync();
        Task<IEnumerable<PhysicalModelDTO>> GetPhysicalModelsAsync();

        Task InsertAsync(RequirementRuleDTO rule);
    }
}