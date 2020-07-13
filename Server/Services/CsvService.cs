using CsvHelper;
using CsvHelper.Configuration;
using FeatureDBPortal.Shared;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CsvService : IAsyncCsvService
    {
        async public Task<byte[]> BuildCsv(CsvExportDTO csvExport)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.CurrentCulture)))
            {
                foreach (var row in csvExport.Combination.Rows)
                {
                    csvWriter.WriteRecord(row);
                    csvWriter.WriteRecords(row.Cells);
                }
                await csvWriter.FlushAsync();
            }

            byte[] csv = memoryStream.ToArray();

            return await Task.FromResult(csv);
        }
    }
}
