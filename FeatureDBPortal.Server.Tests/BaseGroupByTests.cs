using FeatureDBPortal.Server.Data.Models;
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
            var result = await CombinationGroupService.Combine(new Shared.CombinationSearchDTO
            {
                Model = IsGroupListContains(LayoutType.LogicalModel, groups) ? null : new ModelDTO { Id = combinationTest.Model.Id },
                Country = IsGroupListContains(LayoutType.Country, groups) ? null : new CountryDTO { Id = combinationTest.Country.Id },
                Probe = IsGroupListContains(LayoutType.Probe, groups) ? null : new ProbeDTO { Id = combinationTest.Probe.Id },
                Option = IsGroupListContains(LayoutType.Option, groups) ? null : new OptionDTO { Id = combinationTest.Option.Id },
                Application = IsGroupListContains(LayoutType.Application, groups) ? null : new ApplicationDTO { Id = combinationTest.Application.Id },
                Version = IsGroupListContains(LayoutType.MinorVersionAssociation, groups) ? null : new VersionDTO { Id = combinationTest.Version.Id },
                Kit = IsGroupListContains(LayoutType.BiopsyKits, groups) ? null : new KitDTO { Id = combinationTest.Kit.Id },
                UserLevel = combinationTest.User
            },
            groups);

            return result;
        }

        protected abstract ICombinationGroupService BuildCombinationGroupService();
    }
}
