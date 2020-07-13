﻿using CsvHelper;
using CsvHelper.Configuration;
using FeatureDBPortal.Server.Services;
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
        private readonly IAsyncCsvService _csvService;

        public CsvExportController(IAsyncCsvService csvService)
        {
            _csvService = csvService;
        }

        [HttpPost]
        async public Task<FileStreamResult> Post([FromBody]CsvExportDTO csvExport)
        {
            byte[] csv = await _csvService.BuildCsv(csvExport);
            return File(new MemoryStream(csv), "application/octet-stream");
        }
    }
}
