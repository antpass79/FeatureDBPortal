using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : BaseInputFilterController<Country, CountryDTO, CountryController>
    {
        public CountryController(
            ILogger<CountryController> logger,
            IMapper mapper,
            IGenericRepository<Country> repository)
            : base(logger, mapper, repository)
        {
        }
    }
}
