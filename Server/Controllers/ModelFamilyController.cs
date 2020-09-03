using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelFamilyController : BaseInputFilterController<LogicalModel, ModelFamilyDTO, ModelFamilyController>
    {
        public ModelFamilyController(
            ILogger<ModelFamilyController> logger,
            IMapper mapper,
            IGenericRepository<LogicalModel> repository)
            : base(logger, mapper, repository)
        {
        }

        protected override IEnumerable<LogicalModel> PreFilter(IEnumerable<LogicalModel> entities)
        {
            return entities
                .GroupBy(item => item.ModelFamily)
                .Select(group => group.First());
        }
    }
}
