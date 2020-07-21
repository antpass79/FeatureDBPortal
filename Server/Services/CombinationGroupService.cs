using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public abstract class CombinationGroupService : ICombinationGroupService
    {
        private readonly IVersionProvider _versionProvider;
        private readonly GroupProviderBuilder _groupProviderBuilder;

        protected FeaturesContext Context { get; }

        protected CombinationGroupService(FeaturesContext context, IVersionProvider versionProvider, GroupProviderBuilder groupProviderBuilder)
        {
            Context = context;
            _versionProvider = versionProvider;
            _groupProviderBuilder = groupProviderBuilder;
        }

        async public Task<CombinationDTO> GetCombination(CombinationSearchDTO search)
        {
            var filteredNormalRules = FilterNormalRules(search);
            if (filteredNormalRules == null || filteredNormalRules.Count() == 0)
                return await Task.FromResult<CombinationDTO>(null);

            return await BuildCombination(search, filteredNormalRules, _groupProviderBuilder);
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
        IQueryable<NormalRule> FilterNormalRules(CombinationSearchDTO search)
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

        abstract protected Task<CombinationDTO> BuildCombination(CombinationSearchDTO search, IQueryable<NormalRule> normalRules, GroupProviderBuilder groupProviderBuilder);

        private bool IsOutputLayoutTypeSelected(CombinationSearchDTO search, LayoutTypeDTO layoutType) => search.RowLayout == layoutType || search.ColumnLayout == layoutType || search.CellLayout == layoutType;
    }
}
