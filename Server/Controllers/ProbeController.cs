using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Server.Services;
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
    public class ProbeController : BaseInputFilterController<Probe, ProbeDTO, ProbeController>
    {
        public ProbeController(
            ILogger<ProbeController> logger,
            IMapper mapper,
            IGenericRepository<Probe> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<ProbeDTO> PreManipulation(IQueryable<Probe> query)
        {
            return query
                .OrderBy(item => item.SaleName)
                .Select(item => new ProbeDTO { Id = item.Id, Name = item.SaleName });
        }
    }
}
