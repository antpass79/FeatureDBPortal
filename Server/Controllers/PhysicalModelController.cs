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
    public class PhysicalModelController : BaseInputFilterController<PhysicalModel, PhysicalModelDTO, PhysicalModelController>
    {
        public PhysicalModelController(
            ILogger<PhysicalModelController> logger,
            IMapper mapper,
            IGenericRepository<PhysicalModel> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<PhysicalModelDTO> PreManipulation(IQueryable<PhysicalModel> query)
        {
            return query
                .OrderBy(item => item.Description)
                .Select(item => new PhysicalModelDTO { Id = item.Id, Name = item.Description });
        }
    }
}
