using AutoMapper;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class AvailabilityCombinationService : IAvailabilityCombinationService
    {
        private readonly FeaturesContext _context;
        private readonly IMapper _mapper;

        public AvailabilityCombinationService(IMapper mapper, DbContext context)
        {
            _mapper = mapper;
            _context = context as FeaturesContext;
        }

        async public Task<IEnumerable<CombinationDTO>> Get(CombinationSearchDTO search)
        {
            var normalRules = _context
                .NormalRule
                .Where(item =>
                (item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue) &&
                (item.CountryId == search.Country.Id || !item.CountryId.HasValue) &&
                (item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue) &&
                (item.ApplicationId == search.Application.Id || !item.ApplicationId.HasValue) &&
                (item.ProbeId == search.Probe.Id || !item.ProbeId.HasValue) &&
                (item.KitId == search.Kit.Id || !item.KitId.HasValue) &&
                (item.OptionId == search.Option.Id || !item.OptionId.HasValue)).ToList();

            var allow = normalRules.Count > 0 && !normalRules.Any(item => item.Allow == 0);

            var combination = new CombinationDTO
            {
                Description = "",
                Allow = allow
            };

            return await Task.FromResult(new List<CombinationDTO> { combination });
        }
    }
}
