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
    public class OptionController : BaseInputFilterController<Option, OptionDTO, OptionController>
    {
        public OptionController(
            ILogger<OptionController> logger,
            IMapper mapper,
            IGenericRepository<Option> repository)
            : base(logger, mapper, repository)
        {
        }

        protected override IEnumerable<Option> PreFilter(IEnumerable<Option> entities)
        {
            return entities
                .Where(item => !item.IsFake);
        }
    }
}
