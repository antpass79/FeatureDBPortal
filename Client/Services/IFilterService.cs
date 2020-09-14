using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface IFilterService
    {
        Task<IEnumerable<ApplicationDTO>> GetApplicationsAsync();
        Task<IEnumerable<ProbeDTO>> GetProbesAsync();
        Task<IEnumerable<CountryDTO>> GetCountriesAsync();
        Task<IEnumerable<VersionDTO>> GetVersionsAsync();
        Task<IEnumerable<ModelDTO>> GetModelsAsync();
        Task<IEnumerable<ModelFamilyDTO>> GetModelFamiliesAsync();
        Task<IEnumerable<PhysicalModelDTO>> GetPhysicalModelsAsync();
        Task<IEnumerable<OptionDTO>> GetOptionsAsync();
        Task<IEnumerable<KitDTO>> GetKitsAsync();
        Task<IEnumerable<DistributorDTO>> GetDistributorsAsync();
        Task<IEnumerable<CertifierDTO>> GetCertifiersAsync();
        Task<IEnumerable<UserLevelDTO>> GetUsersAsync();
    }
}