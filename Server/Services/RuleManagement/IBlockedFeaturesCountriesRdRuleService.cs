using FeatureDBPortal.Shared.RuleManagement;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services.RuleManagement
{
    public interface IBlockedFeaturesCountriesRdRuleService
    {
        Task InsertAsync(BlockedFeaturesCountriesRdRuleDTO rule);
    }
}