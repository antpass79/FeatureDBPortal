using FeatureDBPortal.Shared.RuleManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services.RuleManagement
{
    public interface IRequirementRuleService
    {
        Task<IEnumerable<RequirementRuleDTO>> GetAsync();
        Task InsertAsync(RequirementRuleDTO rule);
    }
}