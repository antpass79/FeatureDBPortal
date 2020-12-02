using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public class DatabaseService : IDatabaseService
    {
        const string DATABASE_ENDPOINT = "api/database";
        const string DATABASE_ENDPOINT_UPLOAD = "api/database/upload";
        const string DATABASE_ENDPOINT_CONNECT = "api/database/connect";
        const string DATABASE_ENDPOINT_DISCONNECT = "api/database/disconnect";

        private readonly HttpClient _httpClient;

        public DatabaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool Connected { get; private set; } = false;
        public string CurrentDatabase { get; private set; } = string.Empty;

        async public Task<IEnumerable<string>> GetDatabaseNamesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<string>>(DATABASE_ENDPOINT);
        }

        async public Task ConnectAsync(string databaseName)
        {
            try
            {
                await _httpClient.PostAsJsonAsync(DATABASE_ENDPOINT_CONNECT, databaseName);
                OnDatabaseConnectionChanged(databaseName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                OnDatabaseConnectionChanged(string.Empty);
            }
        }

        async public Task<string> UploadAsync(Stream database)
        {
            var content = new MultipartFormDataContent();
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            content.Add(new StreamContent(database, Convert.ToInt32(database.Length)), "database");

            var result = await _httpClient.PostAsync(DATABASE_ENDPOINT_UPLOAD, content);

            return await result.Content.ReadAsStringAsync();
        }

        async public Task DisconnectAsync()
        {
            try
            {
                await _httpClient.DeleteAsync(DATABASE_ENDPOINT_DISCONNECT);
                OnDatabaseConnectionChanged(string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public event EventHandler<DatabaseConnectionEventArgs> DatabaseConnectionChanged;

        void OnDatabaseConnectionChanged(string databaseName)
        {
            var args = new DatabaseConnectionEventArgs(databaseName);
            CurrentDatabase = args.CurrentDatabase;
            Connected = args.Connected;

            DatabaseConnectionChanged?.Invoke(this, args);
        }
    }
}
