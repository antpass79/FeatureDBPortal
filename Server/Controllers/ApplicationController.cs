using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        protected override IQueryable<ApplicationDTO> PreManipulation(IQueryable<Application> query)
        {
            return query
                .Where(item => !item.IsFake)
                .OrderBy(item => item.Name)
                .Select(item => new ApplicationDTO { Id = item.Id, Name = item.Name });
        }
    }
}
