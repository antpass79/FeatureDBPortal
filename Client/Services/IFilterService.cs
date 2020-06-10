using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface IFilterService
    {
        Task<IEnumerable<ApplicationDTO>> GetApplications();
        Task<IEnumerable<ProbeDTO>> GetProbes();
        Task<IEnumerable<CountryDTO>> GetCountries();
        Task<IEnumerable<VersionDTO>> GetVersions();
        Task<IEnumerable<ModelDTO>> GetModels();
        Task<IEnumerable<OptionDTO>> GetOptions();
        Task<IEnumerable<KitDTO>> GetKits();
    }
}
