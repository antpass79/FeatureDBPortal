using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Server.Services.RuleManagement;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.RuleManagement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers.RuleManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinorVersionRuleController : ControllerBase
    {
        private readonly IMinorVersionRuleService _ruleService;

        public MinorVersionRuleController(IMinorVersionRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        [HttpGet]
        async public Task<IEnumerable<MinorVersionRuleDTO>> Get()
        {
            return await _ruleService.GetAsync();
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody] MinorVersionRuleDTO rule)
        {
            await _ruleService.InsertAsync(rule);

            return await Task.FromResult(Ok());
        }
    }
}