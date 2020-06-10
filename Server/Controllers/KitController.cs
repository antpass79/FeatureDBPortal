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
    public class KitController : BaseInputFilterController<BiopsyKits, KitDTO, KitController>
    {
        public KitController(
            ILogger<KitController> logger,
            IMapper mapper,
            IGenericRepository<BiopsyKits> repository)
            : base(logger, mapper, repository)
        {
        }
    }
}
