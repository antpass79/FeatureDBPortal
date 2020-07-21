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
    public class CountryController : BaseInputFilterController<Country, CountryDTO, CountryController>
    {
        public CountryController(
            ILogger<CountryController> logger,
            IMapper mapper,
            IGenericRepository<Country> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<CountryDTO> PreManipulation(IQueryable<Country> query)
        {
            return query
                .OrderBy(item => item.Name)
                .Select(item => new CountryDTO { Id = item.Id, CountryName = item.Name, Code = item.Code });
        }
    }
}
