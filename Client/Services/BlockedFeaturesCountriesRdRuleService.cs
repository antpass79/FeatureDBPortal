using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public class BlockedFeaturesCountriesRdRuleService : IBlockedFeaturesCountriesRdRuleService
    {
        const string RULE_ENDPOINT = "api/blockedfeaturescountriesrdrule";

        private readonly HttpClient _httpClient;
        private readonly IFilterService _filterService;

        public BlockedFeaturesCountriesRdRuleService(HttpClient httpClient, IFilterService filterService)
        {
            _httpClient = httpClient;
            _filterService = filterService;
        }

        async public Task<IEnumerable<ApplicationDTO>> GetApplicationsAsync()
        {
            return await _filterService.GetApplicationsAsync();
        }

        async public Task<IEnumerable<CertifierDTO>> GetCertifiersAsync()
        {
            return await _filterService.GetCertifiersAsync();
        }

        async public Task<IEnumerable<CountryDTO>> GetCountriesAsync()
        {
            return await _filterService.GetCountriesAsync();
        }

        async public Task<IEnumerable<DistributorDTO>> GetDistributorsAsync()
        {
            return await _filterService.GetDistributorsAsync();
        }

        async public Task<IEnumerable<KitDTO>> GetKitsAsync()
        {
            return await _filterService.GetKitsAsync();
        }

        async public Task<IEnumerable<ModelFamilyDTO>> GetModelFamiliesAsync()
        {
            return await _filterService.GetModelFamiliesAsync();
        }

        async public Task<IEnumerable<ModelDTO>> GetModelsAsync()
        {
            return await _filterService.GetModelsAsync();
        }

        async public Task<IEnumerable<OptionDTO>> GetOptionsAsync()
        {
            return await _filterService.GetOptionsAsync();
        }

        async public Task<IEnumerable<ProbeDTO>> GetProbesAsync()
        {
            return await _filterService.GetProbesAsync();
        }

        async public Task<IEnumerable<UserLevelDTO>> GetUsersAsync()
        {
            return await _filterService.GetUsersAsync();
        }

        async public Task InsertAsync(BlockedFeaturesCountriesRdRuleDTO rule)
        {
            var content = new StringContent(JsonSerializer.Serialize(rule), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(RULE_ENDPOINT, content);
        }
    }
}