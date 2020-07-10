using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Shared;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public class CsvExportService : ICsvExportService
    {
        const string CSV_EXPORT_ENDPOINT = "api/csvexport";
        const string SCRIPT_NAME = "SaveFileAs";
        
        private readonly IJSRuntime _jsRuntime;

        public CsvExportService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        private readonly HttpClient _httpClient;

        async public Task DownloadCsv(CombinationDTO combination, string downloadedFile)
        {
            var message = await _httpClient.PostAsync(
                CSV_EXPORT_ENDPOINT,
                new StringContent(JsonSerializer.Serialize(combination), Encoding.UTF8, "application/json"));

            var csv = await message.GetByteArrayValue();
            await DownloadCsv(csv, downloadedFile);
        }

        async private Task DownloadCsv(byte[] csv, string downloadedFile)
        {
            await _jsRuntime.InvokeAsync<object>(
                SCRIPT_NAME,
                downloadedFile,
                Encoding.UTF8.GetString(csv));
        }
    }
}