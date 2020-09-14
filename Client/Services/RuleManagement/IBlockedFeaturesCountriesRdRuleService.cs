using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.RuleManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services.RuleManagement
{
    public interface IBlockedFeaturesCountriesRdRuleService
    {
        Task<IEnumerable<OptionDTO>> GetOptionsAsync();
        Task<IEnumerable<ApplicationDTO>> GetApplicationsAsync();
        Task<IEnumerable<ProbeDTO>> GetProbesAsync();
        Task<IEnumerable<KitDTO>> GetKitsAsync();
        Task<IEnumerable<ModelDTO>> GetModelsAsync();
        Task<IEnumerable<ModelFamilyDTO>> GetModelFamiliesAsync();
        Task<IEnumerable<CountryDTO>> GetCountriesAsync();
        Task<IEnumerable<DistributorDTO>> GetDistributorsAsync();
        Task<IEnumerable<CertifierDTO>> GetCertifiersAsync();
        Task<IEnumerable<UserLevelDTO>> GetUsersAsync();

        Task InsertAsync(BlockedFeaturesCountriesRdRuleDTO rule);
    }
}