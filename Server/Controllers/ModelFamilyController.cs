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
    public class ModelFamilyController : BaseInputFilterController<LogicalModel, ModelFamilyDTO, ModelFamilyController>
    {
        public ModelFamilyController(
            ILogger<ModelFamilyController> logger,
            IMapper mapper,
            IGenericRepository<LogicalModel> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<ModelFamilyDTO> PreManipulation(IQueryable<LogicalModel> query)
        {
            return query
                .AsEnumerable()
                .GroupBy(item => item.ModelFamily)
                .OrderBy(item => item.Key)
                .Select(group => group.First())
                .Select(item => new ModelFamilyDTO { Id = item.Id, Name = item.ModelFamily })
                .AsQueryable();
        }
    }
}