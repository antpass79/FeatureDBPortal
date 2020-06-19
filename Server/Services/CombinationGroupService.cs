using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public abstract class CombinationGroupService : ICombinationGroupService
    {
        protected FeaturesContext Context { get; }

        protected CombinationGroupService(DbContext context)
        {
            Context = context as FeaturesContext;
        }

        public abstract Task<CombinationDTO> Combine(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy);

        protected IEnumerable<NormalRule> FilterNormalRules(CombinationSearchDTO search)
        {
            return Context
                .NormalRule
                    .WhereIf(item => item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue, search.Model != null)
                    .WhereIf(item => item.CountryId == search.Country.Id || !item.CountryId.HasValue, search.Country != null)
                    .WhereIf(item => item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue, search.UserLevel != UserLevelDTO.None)
                    .WhereIf(item => item.ProbeId == search.Probe.Id || !item.ProbeId.HasValue, search.Probe != null)
                    .WhereIf(item => item.KitId == search.Kit.Id || !item.KitId.HasValue, search.Kit != null)
                    .WhereIf(item => item.OptionId == search.Option.Id || !item.OptionId.HasValue, search.Option != null)
                    .WhereIf(item => item.ApplicationId == search.Application.Id || !item.ApplicationId.HasValue, search.Application != null);
        }
    }
}
