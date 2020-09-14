using FeatureDBPortal.Shared.RuleManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services.RuleManagement
{
    public interface IMinorVersionRuleService
    {
        Task<IEnumerable<MinorVersionRuleDTO>> GetAsync();
        Task InsertAsync(MinorVersionRuleDTO rule);
    }
}