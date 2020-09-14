using FeatureDBPortal.Shared.RuleManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services.RuleManagement
{
    public class RequirementRuleService : IRequirementRuleService
    {
        async public Task<IEnumerable<RequirementRuleDTO>> GetAsync()
        {
            var result = new List<RequirementRuleDTO>
            {
                new RequirementRuleDTO { },
                new RequirementRuleDTO { },
                new RequirementRuleDTO { },
                new RequirementRuleDTO { },
                new RequirementRuleDTO { }
            };

            return await Task.FromResult<IEnumerable<RequirementRuleDTO>>(result);
        }

        async public Task InsertAsync(RequirementRuleDTO rule)
        {
            await Task.CompletedTask;
        }
    }
}