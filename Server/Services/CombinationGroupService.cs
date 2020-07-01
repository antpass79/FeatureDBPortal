using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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

        protected IQueryable<NormalRule> FilterNormalRules(CombinationSearchDTO search)
        {
            return Context
                .NormalRule
            //.Where(normalRule => !normalRule.LogicalModelId.HasValue || (search.Model != null && search.Model.Id == normalRule.LogicalModelId))
            //.Where(normalRule => !normalRule.CountryId.HasValue || (search.Country != null && search.Country.Id == normalRule.CountryId))
            //.Where(normalRule => !normalRule.UserLevel.HasValue || (search.UserLevel != UserLevelDTO.None && search.UserLevel == (UserLevelDTO)normalRule.UserLevel))
            //.Where(normalRule => !normalRule.ProbeId.HasValue || (search.Probe != null && search.Probe.Id == normalRule.ProbeId))
            //.Where(normalRule => !normalRule.KitId.HasValue || (search.Kit != null && search.Kit.Id == normalRule.KitId))
            //.Where(normalRule => !normalRule.OptionId.HasValue || (search.Option != null && search.Option.Id == normalRule.OptionId))
            //.Where(normalRule => !normalRule.ApplicationId.HasValue || (search.Application != null && search.Application.Id == normalRule.ApplicationId));

            .WhereIf(normalRule => normalRule.LogicalModelId == search.Model.Id || !normalRule.LogicalModelId.HasValue, search.Model != null)
            .WhereIf(normalRule => normalRule.CountryId == search.Country.Id || !normalRule.CountryId.HasValue, search.Country != null)
            .WhereIf(normalRule => normalRule.UserLevel == (short)search.UserLevel || !normalRule.UserLevel.HasValue, search.UserLevel != UserLevelDTO.None)
            .WhereIf(normalRule => normalRule.ProbeId == search.Probe.Id || !normalRule.ProbeId.HasValue, search.Probe != null)
            .WhereIf(normalRule => normalRule.KitId == search.Kit.Id || !normalRule.KitId.HasValue, search.Kit != null)
            .WhereIf(normalRule => normalRule.OptionId == search.Option.Id || !normalRule.OptionId.HasValue, search.Option != null)
            .WhereIf(normalRule => normalRule.ApplicationId == search.Application.Id || !normalRule.ApplicationId.HasValue, search.Application != null);
        }

        protected int? GetPropertyValue(NormalRule normalRule, string propertyName)
        {
            var result = propertyName switch
            {
                "ApplicationId" => normalRule.ApplicationId,
                "ProbeId" => normalRule.ProbeId,
                "OptionId" => normalRule.OptionId,
                _ => throw new NotSupportedException()
            };

            return result;
        }

    }
}
