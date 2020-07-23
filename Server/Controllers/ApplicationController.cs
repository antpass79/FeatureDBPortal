using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Server.Services;
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
            IGenericRepository<Application> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<ApplicationDTO> PreManipulation(IQueryable<Application> query)
        {
            return query
                .OrderBy(item => item.Name)
                .Select(item => new ApplicationDTO { Id = item.Id, Name = item.Name, IsFake = item.IsFake });
        }
    }
}
