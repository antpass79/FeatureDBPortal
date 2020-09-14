using FeatureDBPortal.Shared.RuleManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services.RuleManagement
{
    public interface IMinorVersionRuleService
    {
        Task<IEnumerable<MinorVersionRuleDTO>> GetAllAsync();
        Task InsertAsync(MinorVersionRuleDTO rule);
    }
}