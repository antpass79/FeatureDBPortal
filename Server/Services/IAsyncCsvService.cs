using FeatureDBPortal.Shared;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface IAsyncCsvService
    {
        Task<byte[]> BuildCsv(CsvExportDTO csvExport);
    }
}
