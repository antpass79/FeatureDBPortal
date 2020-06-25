using AutoMapper;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityCombinationController : ControllerBase
    {
        private readonly ILogger<AvailabilityCombinationController> _logger;
        private readonly IAvailabilityCombinationService _availabilityCombinationService;

        public AvailabilityCombinationController(
            ILogger<AvailabilityCombinationController> logger,
            IAvailabilityCombinationService availabilityCombinationService)
        {
            _logger = logger;
            _availabilityCombinationService = availabilityCombinationService;
        }

        [HttpPost]
        async public Task<IActionResult> Get([FromBody]CombinationSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                           .Where(y => y.Count > 0)
                                           .ToList();
                return BadRequest(errors);
            }

            return Ok(await _availabilityCombinationService.Get(search));
        }
    }
}
