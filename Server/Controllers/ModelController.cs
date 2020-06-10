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
    public class ModelController : BaseInputFilterController<LogicalModel, ModelDTO, ModelController>
    {
        public ModelController(
            ILogger<ModelController> logger,
            IMapper mapper,
            IGenericRepository<LogicalModel> repository)
            : base(logger, mapper, repository)
        {
        }
    }
}
