using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Shared;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public class AvailabilityCombinationService : IAvailabilityCombinationService
    {
        const string AVAILABILITY_COMBINATION_ENDPOINT = "api/availabilitycombination";

        private readonly HttpClient _httpClient;

        public AvailabilityCombinationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async public Task<CombinationDTO> GetCombinations(CombinationSearchDTO search)
        {
            var message = await _httpClient.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(search), Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_httpClient.BaseAddress + AVAILABILITY_COMBINATION_ENDPOINT)
            });

            return await message.GetValue<CombinationDTO>();
        }
    }
}
