using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        public IActionResult Get()
        {
            byte[] buffer;
         
            var memoryStream = new System.IO.MemoryStream();
            buffer = Encoding.UTF8.GetBytes("Sample text");
            return File(buffer, "application/force-download", "combination.csv");
        }
    }
}
