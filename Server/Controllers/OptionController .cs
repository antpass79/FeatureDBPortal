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
    public class OptionController : BaseInputFilterController<Option, OptionDTO, OptionController>
    {
        public OptionController(
            ILogger<OptionController> logger,
            IMapper mapper,
            IGenericRepository<Option> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<OptionDTO> PreManipulation(IQueryable<Option> query)
        {
            return query
                .OrderBy(item => item.Name)
                .Select(item => new OptionDTO { Id = item.Id, Name = item.Name, IsFake = item.IsFake });
        }
    }
}
