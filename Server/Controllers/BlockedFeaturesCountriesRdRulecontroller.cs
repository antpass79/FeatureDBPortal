using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockedFeaturesCountriesRdRulecontroller : ControllerBase
    {
        public BlockedFeaturesCountriesRdRulecontroller()
        {
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody] BlockedFeaturesCountriesRdRuleDTO rule)
        {
            return await Task.FromResult(Ok());
        }
    }
}