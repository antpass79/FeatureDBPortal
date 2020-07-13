using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Server.Tests.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Tests
{
    public abstract class BaseGroupByTests
    {
        private ICombinationGroupService CombinationGroupService { get; set; }

        protected BaseGroupByTests()
        {
            CombinationGroupService = BuildCombinationGroupService();
        }

        private bool IsGroupListContains(LayoutType layoutType, List<LayoutType> groups) => groups.Contains(layoutType);

        async protected Task<CombinationDTO> GroupBy(CombinationTest combinationTest, List<LayoutType> groups)
        {
            var result = await CombinationGroupService.GetCombination(new Shared.CombinationSearchDTO
            {
                ModelId = IsGroupListContains(LayoutType.LogicalModel, groups) ? null : combinationTest.ModelId,
                CountryId = IsGroupListContains(LayoutType.Country, groups) ? null : combinationTest.CountryId,
                ProbeId = IsGroupListContains(LayoutType.Probe, groups) ? null : combinationTest.ProbeId,
                OptionId = IsGroupListContains(LayoutType.Option, groups) ? null : combinationTest.OptionId,
                ApplicationId = IsGroupListContains(LayoutType.Application, groups) ? null : combinationTest.ApplicationId,
                VersionId = IsGroupListContains(LayoutType.MinorVersionAssociation, groups) ? null : combinationTest.VersionId,
                KitId = IsGroupListContains(LayoutType.BiopsyKits, groups) ? null : combinationTest.KitId,
                UserLevel = combinationTest.User
            });

            return result;
        }

        protected abstract ICombinationGroupService BuildCombinationGroupService();
    }
}
