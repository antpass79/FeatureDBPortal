using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.RuleManagement;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services.RuleManagement
{
    public class RequirementRuleService : IRequirementRuleService
    {
        const string RULE_ENDPOINT = "api/requirementrule";

        private readonly HttpClient _httpClient;
        private readonly IFilterService _filterService;

        public RequirementRuleService(HttpClient httpClient, IFilterService filterService)
        {
            _httpClient = httpClient;
            _filterService = filterService;
        }

        async public Task<IEnumerable<ApplicationDTO>> GetApplicationsAsync()
        {
            return await _filterService.GetApplicationsAsync();
        }

        async public Task<IEnumerable<KitDTO>> GetKitsAsync()
        {
            return await _filterService.GetKitsAsync();
        }

        async public Task<IEnumerable<PhysicalModelDTO>> GetPhysicalModelsAsync()
        {
            return await _filterService.GetPhysicalModelsAsync();
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

        async public Task InsertAsync(RequirementRuleDTO rule)
        {
            await _httpClient.PostAsJsonAsync(RULE_ENDPOINT, rule);
        }
    }
}