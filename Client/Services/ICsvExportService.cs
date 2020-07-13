using FeatureDBPortal.Shared;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface ICsvExportService
    {
        Task DownloadCsv(CsvExportDTO csvExport);
    }
}
