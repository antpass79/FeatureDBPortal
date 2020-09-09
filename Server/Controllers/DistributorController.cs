using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistributorController : BaseInputFilterController<Distributor, DistributorDTO, DistributorController>
    {
        public DistributorController(
            ILogger<DistributorController> logger,
            IMapper mapper,
            IGenericRepository<Distributor> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<DistributorDTO> PreManipulation(IQueryable<Distributor> query)
        {
            return query
                .OrderBy(item => item.Name)
                .Select(item => new DistributorDTO { Id = item.Id, Name = item.Name });
        }
    }
}