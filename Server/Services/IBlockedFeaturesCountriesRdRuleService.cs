using FeatureDBPortal.Shared;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface IBlockedFeaturesCountriesRdRuleService
    {
        Task Insert(BlockedFeaturesCountriesRdRuleDTO rule);
    }
}
