using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : BaseInputFilterController<MinorVersionAssociation, VersionDTO, VersionController>
    {
        private readonly IVersionProvider _versionProvider;

        public VersionController(
            ILogger<VersionController> logger,
            IMapper mapper,
            IGenericRepository<MinorVersionAssociation> repository,
            IVersionProvider versionProvider)
            : base(logger, mapper, repository)
        {
            _versionProvider = versionProvider;
        }

        protected override IQueryable<VersionDTO> PreManipulation(IQueryable<MinorVersionAssociation> query)
        {
            return query
                .OrderBy(item => item.BuildVersion)
                .Select(item => new VersionDTO { Id = item.Id, BuildVersion = item.BuildVersion });
        }

        protected override IEnumerable<VersionDTO> PostManipulation(IEnumerable<VersionDTO> items)
        {
            return _versionProvider.Versions.Select(version => new VersionDTO
            {
                Id = version.Id,
                BuildVersion = version.Name
            });
        }
    }
}
