using FeatureDBPortal.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly IAsyncDatabaseService _databaseService;

        public DatabaseController(IAsyncDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        async public Task<IEnumerable<string>> Get()
        {
            return await _databaseService.GetDatabaseNamesAsync();
        }

        [HttpPost]
        [Route("Upload")]
        async public Task<IActionResult> Upload([FromBody] byte[] database)
        {
            await _databaseService.UploadAsync(database);

            return await Task.FromResult(Ok());
        }

        [HttpPost]
        [Route("Connect")]
        async public Task<IActionResult> Connect([FromBody] string databaseName)
        {
            await _databaseService.ConnectAsync(databaseName);

            return await Task.FromResult(Ok());
        }
    }
}
