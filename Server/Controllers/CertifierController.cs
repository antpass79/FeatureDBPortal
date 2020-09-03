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
    public class CertifierController : BaseInputFilterController<Certifier, CertifierDTO, CertifierController>
    {
        public CertifierController(
            ILogger<CertifierController> logger,
            IMapper mapper,
            IGenericRepository<Certifier> repository)
            : base(logger, mapper, repository)
        {
        }
    }
}
