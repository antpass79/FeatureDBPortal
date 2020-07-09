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
    public class ApplicationController : BaseInputFilterController<Application, ApplicationDTO, ApplicationController>
    {
        public ApplicationController(
            ILogger<ApplicationController> logger,
            IMapper mapper,
            IGenericRepository<Application> repository)
            : base(logger, mapper, repository)
        {
        }

        protected override IEnumerable<Application> PreFilter(IEnumerable<Application> entities)
        {
            return entities
                .Where(item => !item.IsFake);
        }
    }
}
