using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Shared;
using Microsoft.JSInterop;
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

        async public Task DownloadCsv(CsvExportDTO csvExport)
        {
            var message = await _httpClient.PostAsync(
                CSV_EXPORT_ENDPOINT,
                new StringContent(JsonSerializer.Serialize(csvExport), Encoding.UTF8, "application/json"));

            var csv = await message.GetByteArrayValue();
            await DownloadCsv(csv, csvExport.Settings);
        }

        async private Task DownloadCsv(byte[] csv, CsvExportSettingsDTO csvExportSettings)
        {
            var fileName = !string.IsNullOrWhiteSpace(csvExportSettings.FileName) ? $"{csvExportSettings.FileName}.csv" : $"{csvExportSettings.DefaultFileName}.csv";

            await _jsRuntime.InvokeAsync<object>(
                SCRIPT_NAME,
                fileName,
                Encoding.UTF8.GetString(csv));
        }
    }
}