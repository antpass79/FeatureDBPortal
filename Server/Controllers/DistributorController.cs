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
    public class DistributorController : BaseInputFilterController<Distributor, DistributorDTO, DistributorController>
    {
        public DistributorController(
            ILogger<DistributorController> logger,
            IMapper mapper,
            IGenericRepository<Distributor> repository)
            : base(logger, mapper, repository)
        {
        }
    }
}
