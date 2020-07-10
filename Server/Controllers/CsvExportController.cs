using CsvHelper;
using CsvHelper.Configuration;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CsvExportController : ControllerBase
    {
        public CsvExportController()
        {
        }

        [HttpPost]
        async public Task<FileStreamResult> Post([FromBody]CombinationDTO combination)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.CurrentCulture)))
            {
                foreach (var row in combination.Rows)
                {
                    csvWriter.WriteRecord(row);
                    csvWriter.WriteRecords(row.Cells);
                }
                await csvWriter.FlushAsync();
            }

            //byte[] buffer = Encoding.UTF8.GetBytes("Sample text");
            byte[] buffer = memoryStream.ToArray();

            //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //result.Content = new ByteArrayContent(buffer);
            //result.Content.Headers.ContentType =
            //    new MediaTypeHeaderValue("application/octet-stream");

            return File(new MemoryStream(buffer), "application/octet-stream");
            //return await Task.FromResult(result);
        }
    }
}
