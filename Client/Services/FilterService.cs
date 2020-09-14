using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public class FilterService : IFilterService
    {
        const string APPLICATION_ENDPOINT = "api/application";
        const string PROBE_ENDPOINT = "api/probe";
        const string COUNTRY_ENDPOINT = "api/country";
        const string VERSION_ENDPOINT = "api/version";
        const string MODEL_ENDPOINT = "api/model";
        const string MODEL_FAMILY_ENDPOINT = "api/modelfamily";
        const string PHYSICAL_MODEL_ENDPOINT = "api/physicalmodel";
        const string OPTION_ENDPOINT = "api/option";
        const string KIT_ENDPOINT = "api/kit";
        const string DISTRIBUTOR_ENDPOINT = "api/distributor";
        const string CERTIFIER_ENDPOINT = "api/certifier";
        const string USER_ENDPOINT = "api/user";

        private readonly HttpClient _httpClient;

        public FilterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<ApplicationDTO>> GetApplicationsAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<ApplicationDTO>>(APPLICATION_ENDPOINT);
        }

        public Task<IEnumerable<ProbeDTO>> GetProbesAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<ProbeDTO>>(PROBE_ENDPOINT);
        }

        public Task<IEnumerable<CountryDTO>> GetCountriesAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<CountryDTO>>(COUNTRY_ENDPOINT);
        }

        public Task<IEnumerable<VersionDTO>> GetVersionsAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<VersionDTO>>(VERSION_ENDPOINT);
        }

        public Task<IEnumerable<ModelDTO>> GetModelsAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<ModelDTO>>(MODEL_ENDPOINT);
        }

        public Task<IEnumerable<ModelFamilyDTO>> GetModelFamiliesAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<ModelFamilyDTO>>(MODEL_FAMILY_ENDPOINT);
        }

        public Task<IEnumerable<PhysicalModelDTO>> GetPhysicalModelsAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<PhysicalModelDTO>>(PHYSICAL_MODEL_ENDPOINT);
        }

        public Task<IEnumerable<OptionDTO>> GetOptionsAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<OptionDTO>>(OPTION_ENDPOINT);
        }

        public Task<IEnumerable<KitDTO>> GetKitsAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<KitDTO>>(KIT_ENDPOINT);
        }

        public Task<IEnumerable<DistributorDTO>> GetDistributorsAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<DistributorDTO>>(DISTRIBUTOR_ENDPOINT);
        }

        public Task<IEnumerable<CertifierDTO>> GetCertifiersAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<CertifierDTO>>(CERTIFIER_ENDPOINT);
        }

        public Task<IEnumerable<UserLevelDTO>> GetUsersAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<UserLevelDTO>>(USER_ENDPOINT);
        }
    }
}