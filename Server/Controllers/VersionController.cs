using AutoMapper;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : BaseInputFilterController<MinorVersionAssociation, VersionDTO, VersionController>
    {
        public VersionController(
            ILogger<VersionController> logger,
            IMapper mapper,
            IGenericRepository<MinorVersionAssociation> repository)
            : base(logger, mapper, repository)
        {
        }
    }
}
