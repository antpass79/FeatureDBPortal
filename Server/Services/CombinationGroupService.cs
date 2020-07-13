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

        public Task<CombinationDTO> GetCombination(CombinationSearchDTO search)
        {
            var filteredNornalRules = FilterNormalRules(search);
            return GroupNormalRules(search, filteredNornalRules);
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
            if (!search.VersionId.HasValue && search.ModelId.HasValue && search.CountryId.HasValue)
            {
                int version = _versionProvider.BuildDefaultVersion(search.CountryId.Value, search.ModelId.Value);
                search.VersionId = version;
            }

            var result = Context
                .NormalRule
            .WhereIf(normalRule => !normalRule.LogicalModelId.HasValue || search.ModelId.HasValue && search.ModelId == normalRule.LogicalModelId, !IsOutputLayoutTypeSelected(search, LayoutTypeDTO.Model))
            .WhereIf(normalRule => !normalRule.CountryId.HasValue || search.CountryId.HasValue && search.CountryId == normalRule.CountryId, !IsOutputLayoutTypeSelected(search, LayoutTypeDTO.Country))
            .WhereIf(normalRule => !normalRule.UserLevel.HasValue || search.UserLevel == (UserLevelDTO)normalRule.UserLevel, search.UserLevel != UserLevelDTO.None)
            .WhereIf(normalRule => !normalRule.ProbeId.HasValue || search.ProbeId.HasValue && search.ProbeId == normalRule.ProbeId, !IsOutputLayoutTypeSelected(search, LayoutTypeDTO.Probe))
            .WhereIf(normalRule => !normalRule.KitId.HasValue || search.KitId.HasValue && search.KitId == normalRule.KitId, !IsOutputLayoutTypeSelected(search, LayoutTypeDTO.Kit))
            .WhereIf(normalRule => !normalRule.OptionId.HasValue || search.OptionId.HasValue && search.OptionId == normalRule.OptionId,  !IsOutputLayoutTypeSelected(search, LayoutTypeDTO.Option))
            .WhereIf(normalRule => !normalRule.ApplicationId.HasValue || search.ApplicationId.HasValue && search.ApplicationId == normalRule.ApplicationId, !IsOutputLayoutTypeSelected(search, LayoutTypeDTO.Application))
            .WhereIf(normalRule => !normalRule.Version.HasValue || search.VersionId.HasValue && search.VersionId < normalRule.Version, !IsOutputLayoutTypeSelected(search, LayoutTypeDTO.Version));

            return result;
        }

        protected abstract Task<CombinationDTO> GroupNormalRules(CombinationSearchDTO search, IQueryable<NormalRule> normalRules);

        private bool IsOutputLayoutTypeSelected(CombinationSearchDTO search, LayoutTypeDTO layoutType) => search.RowLayout == layoutType || search.ColumnLayout == layoutType || search.CellLayout == layoutType;
    }
}
