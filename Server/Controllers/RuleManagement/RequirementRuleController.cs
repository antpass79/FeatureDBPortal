using FeatureDBPortal.Server.Services.RuleManagement;
using FeatureDBPortal.Shared.RuleManagement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers.RuleManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequirementRuleController : ControllerBase
    {
        private readonly IRequirementRuleService _ruleService;

        public RequirementRuleController(IRequirementRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        [HttpGet]
        async public Task<IEnumerable<RequirementRuleDTO>> Get()
        {
            return await _ruleService.GetAsync();
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody] RequirementRuleDTO rule)
        {
            await _ruleService.InsertAsync(rule);

            return await Task.FromResult(Ok());
        }
    }
}