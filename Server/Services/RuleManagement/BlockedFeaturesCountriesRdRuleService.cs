using FeatureDBPortal.Shared.RuleManagement;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services.RuleManagement
{
    public class BlockedFeaturesCountriesRdRuleService : IBlockedFeaturesCountriesRdRuleService
    {
        async public Task InsertAsync(BlockedFeaturesCountriesRdRuleDTO rule)
        {
            await Task.CompletedTask;
        }
    }
}