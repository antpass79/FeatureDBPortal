using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelController : BaseInputFilterController<LogicalModel, ModelDTO, ModelController>
    {
        public ModelController(
            ILogger<ModelController> logger,
            IMapper mapper,
            IGenericRepository<LogicalModel> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<ModelDTO> PreManipulation(IQueryable<LogicalModel> query)
        {
            return query
                .OrderBy(item => item.Name)
                .Select(item => new ModelDTO { Id = item.Id, Name = item.Name });
        }
    }
}
