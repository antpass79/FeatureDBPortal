using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public class CsvExportService : ICsvExportService
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        async public Task<byte[]> DownloadCsv(CombinationDTO combination)
        {
            await JSRuntime.InvokeAsync<object>(
                   "SaveFileAs",
                   "combination.csv",
                   "prova di testo"
               );

            return await Task.FromResult<byte[]>(null);
        }
    }
}
