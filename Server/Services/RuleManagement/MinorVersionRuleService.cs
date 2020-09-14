using FeatureDBPortal.Shared.RuleManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services.RuleManagement
{
    public class MinorVersionRuleService : IMinorVersionRuleService
    {
        async public Task<IEnumerable<MinorVersionRuleDTO>> GetAsync()
        {
            var result = new List<MinorVersionRuleDTO>
            {
                new MinorVersionRuleDTO { Id = 12, Major = 1, Minor = 3, Patch = 12 },
                new MinorVersionRuleDTO { Id = 15, Major = 1, Minor = 2, Patch = 1 },
                new MinorVersionRuleDTO { Id = 21, Major = 1, Minor = 5, Patch = 9 },
                new MinorVersionRuleDTO { Id = 32, Major = 2, Minor = 1, Patch = 7 },
                new MinorVersionRuleDTO { Id = 65, Major = 3, Minor = 3, Patch = 3 }
            };

            return await Task.FromResult<IEnumerable<MinorVersionRuleDTO>>(result);
        }

        async public Task InsertAsync(MinorVersionRuleDTO rule)
        {
            await Task.CompletedTask;
        }
    }
}