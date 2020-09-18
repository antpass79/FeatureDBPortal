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
        private readonly IAsyncPerUserDatabaseService _perUserDatabaseService;

        public DatabaseController(IAsyncPerUserDatabaseService perUserDatabaseService)
        {
            _perUserDatabaseService = perUserDatabaseService;
        }

        [HttpGet]
        async public Task<IEnumerable<string>> Get()
        {
            return await _perUserDatabaseService.GetDatabaseNamesAsync();
        }

        [HttpPost]
        [Route("Upload")]
        async public Task<IActionResult> Upload([FromBody] byte[] database)
        {
            var result = await _perUserDatabaseService.UploadAsync(User, database);
            return Ok(result);
        }

        [HttpPost]
        [Route("Connect")]
        async public Task<IActionResult> Connect([FromBody] string databaseName)
        {
            await _perUserDatabaseService.ConnectAsync(User, databaseName);

            return await Task.FromResult(Ok());
        }

        [HttpDelete]
        [Route("Disconnect")]
        async public Task<IActionResult> Disconnect()
        {
            await _perUserDatabaseService.DisconnectAsync(User);

            return await Task.FromResult(Ok());
        }
    }
}
