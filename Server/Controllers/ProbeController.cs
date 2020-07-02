using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProbeController : BaseInputFilterController<Probe, ProbeDTO, ProbeController>
    {
        public ProbeController(
            ILogger<ProbeController> logger,
            IMapper mapper,
            IGenericRepository<Probe> repository)
            : base(logger, mapper, repository)
        {
        }
    }
}
