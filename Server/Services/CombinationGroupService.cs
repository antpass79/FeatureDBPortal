using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Server.Utils;
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
        private readonly IVersionProvider _versionProvider;

        protected FeaturesContext Context { get; }

        protected CombinationGroupService(DbContext context, IVersionProvider versionProvider)
        {
            Context = context as FeaturesContext;
            _versionProvider = versionProvider;
        }

        public Task<CombinationDTO> GetCombination(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
        {
            var filteredNornalRules = FilterNormalRules(search);
            return GroupNormalRules(search, filteredNornalRules, groupBy);
        }

        // Check for input parameters
        // if (rule.InputId == null)
        // {
        //     return true;
        // }
        // if (search.Input == null || search.Input.Id == null)
        // {
        //     return false;
        // }
        // return search.InputId == rule.InputId;

        // Check for input version
        // if (rule.InputId == null)
        // {
        //     return true;
        // }
        // if (search.Input == null || search.Input.Id == null)
        // {
        //     return false;
        // }
        // return search.InputId < rule.InputId;
        protected IQueryable<NormalRule> FilterNormalRules(CombinationSearchDTO search)
        {
            if ((search.Version == null || !search.Version.Id.HasValue) &&
                search.Model != null &&
                search.Country != null)
            {
                int version = _versionProvider.BuildDefaultVersion(search.Country.Id.Value, search.Model.Id.Value);
                search.Version = new VersionDTO
                {
                    BuildVersion = _versionProvider.BuildStringVersion(version),
                    Id = version
                };
            }

            var result = Context
                .NormalRule
            .WhereIf(normalRule => !normalRule.LogicalModelId.HasValue || search.Model.Id == normalRule.LogicalModelId, search.Model != null)
            .WhereIf(normalRule => !normalRule.CountryId.HasValue || search.Country.Id == normalRule.CountryId, search.Country != null)
            .WhereIf(normalRule => !normalRule.UserLevel.HasValue || search.UserLevel == (UserLevelDTO)normalRule.UserLevel, search.UserLevel != UserLevelDTO.None)
            .WhereIf(normalRule => !normalRule.ProbeId.HasValue || search.Probe.Id == normalRule.ProbeId, search.Probe != null)
            .WhereIf(normalRule => !normalRule.KitId.HasValue || search.Kit.Id == normalRule.KitId, search.Kit != null)
            .WhereIf(normalRule => !normalRule.OptionId.HasValue || search.Option.Id == normalRule.OptionId, search.Option != null)
            .WhereIf(normalRule => !normalRule.ApplicationId.HasValue || search.Application.Id == normalRule.ApplicationId, search.Application != null)
            .WhereIf(normalRule => !normalRule.Version.HasValue || search.Version.Id.HasValue && search.Version.Id < normalRule.Version, search.Version != null);

            return result;
        }

        protected abstract Task<CombinationDTO> GroupNormalRules(CombinationSearchDTO search, IQueryable<NormalRule> normalRules, IEnumerable<LayoutType> groupBy);
    }
}
