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
        const string OPTION_ENDPOINT = "api/option";
        const string KIT_ENDPOINT = "api/kit";

        private readonly HttpClient _httpClient;

        public FilterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<ApplicationDTO>> GetApplications()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<ApplicationDTO>>(APPLICATION_ENDPOINT);
        }

        public Task<IEnumerable<ProbeDTO>> GetProbes()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<ProbeDTO>>(PROBE_ENDPOINT);
        }

        public Task<IEnumerable<CountryDTO>> GetCountries()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<CountryDTO>>(COUNTRY_ENDPOINT);
        }

        public Task<IEnumerable<VersionDTO>> GetVersions()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<VersionDTO>>(VERSION_ENDPOINT);
        }

        public Task<IEnumerable<ModelDTO>> GetModels()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<ModelDTO>>(MODEL_ENDPOINT);
        }

        public Task<IEnumerable<OptionDTO>> GetOptions()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<OptionDTO>>(OPTION_ENDPOINT);
        }

        public Task<IEnumerable<KitDTO>> GetKits()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<KitDTO>>(KIT_ENDPOINT);
        }
    }
}
