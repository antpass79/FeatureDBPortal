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
    public class MinorVersionRuleService : IMinorVersionRuleService
    {
        const string RULE_ENDPOINT = "api/minorversionrule";

        private readonly HttpClient _httpClient;

        public MinorVersionRuleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async public Task<IEnumerable<MinorVersionRuleDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<MinorVersionRuleDTO>>(RULE_ENDPOINT);
        }

        async public Task InsertAsync(MinorVersionRuleDTO rule)
        {
            await _httpClient.PostAsJsonAsync(RULE_ENDPOINT, rule);
        }
    }
}