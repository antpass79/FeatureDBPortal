using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public class DatabaseService : IDatabaseService
    {
        const string DATABASE_ENDPOINT = "api/database";
        const string DATABASE_ENDPOINT_UPLOAD = "api/database/upload";
        const string DATABASE_ENDPOINT_CONNECT = "api/database/connect";

        private readonly HttpClient _httpClient;

        public DatabaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async public Task<IEnumerable<string>> GetDatabaseNamesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<string>>(DATABASE_ENDPOINT);
        }

        async public Task ConnectAsync(string databaseName)
        {
            await _httpClient.PostAsJsonAsync(DATABASE_ENDPOINT_CONNECT, databaseName);
        }

        async public Task UploadAsync(byte[] database)
        {
            await _httpClient.PostAsJsonAsync(DATABASE_ENDPOINT_UPLOAD, database);
        }
    }
}
