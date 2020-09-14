using FeatureDBPortal.Server.Services.RuleManagement;
using FeatureDBPortal.Shared.RuleManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers.RuleManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockedFeaturesCountriesRdRuleController : ControllerBase
    {
        private readonly IBlockedFeaturesCountriesRdRuleService _ruleService;

        public BlockedFeaturesCountriesRdRuleController(IBlockedFeaturesCountriesRdRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody] BlockedFeaturesCountriesRdRuleDTO rule)
        {
            await _ruleService.InsertAsync(rule);

            return await Task.FromResult(Ok());
        }
    }
}