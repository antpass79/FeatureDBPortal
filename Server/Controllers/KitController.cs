﻿using AutoMapper;
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
    public class KitController : BaseInputFilterController<BiopsyKits, KitDTO, KitController>
    {
        public KitController(
            ILogger<KitController> logger,
            IMapper mapper,
            IGenericRepository<BiopsyKits> repository,
            IFilterCache filterCache)
            : base(logger, mapper, repository, filterCache)
        {
        }

        protected override IQueryable<KitDTO> PreManipulation(IQueryable<BiopsyKits> query)
        {
            return query
                .OrderBy(item => item.Name)
                .Select(item => new KitDTO { Id = item.Id, Name = item.Name });
        }
    }
}
