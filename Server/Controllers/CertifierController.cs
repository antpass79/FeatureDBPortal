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
    public class CertifierController : BaseInputFilterController<Certifier, CertifierDTO, CertifierController>
    {
        public CertifierController(
            ILogger<CertifierController> logger,
            IMapper mapper,
            IGenericRepository<Certifier> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<CertifierDTO> PreManipulation(IQueryable<Certifier> query)
        {
            return query
                .OrderBy(item => item.Name)
                .Select(item => new CertifierDTO { Id = item.Id, Name = item.Name });
        }
    }
}