using FeatureDBPortal.Shared;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class BlockedFeaturesCountriesRdRuleService : IBlockedFeaturesCountriesRdRuleService
    {
        async public Task Insert(BlockedFeaturesCountriesRdRuleDTO rule)
        {
            await Task.CompletedTask;
        }
    }
}
