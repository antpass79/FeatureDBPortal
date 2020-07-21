using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Server.Tests.Attributes;
using FeatureDBPortal.Server.Tests.Models;
using FeatureDBPortal.Server.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FeatureDBPortal.Server.Tests
{
    public class GroupByAnyTests : BaseGroupByTests
    {
        public GroupByAnyTests()
        {
        }

        protected override ICombinationGroupService BuildCombinationGroupService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<FeaturesContext>();
            optionsBuilder.UseSqlServer("Server=PC\\SQLExpress;Database=Features;Trusted_Connection=True;");
            var context = new FeaturesContext(optionsBuilder.Options);

            return new CombinationGroupByAnyService(context, new VersionProvider(context), new GroupProviderBuilder(context, new FilterCache()));
        }

        [Theory]
        [JsonFileData(".//Resources//" + TestConstants.TEST_FILE_GROUP_BY_ANY_WITH_COMBINATIONS)]
        async public void GroupByAnyWithCombination(CombinationTest combinationTest, ExpectedResult expectedResult)
        {
            var result = await GroupBy(combinationTest, new List<LayoutType>());

            CombinationAssert.HeaderEqual(expectedResult, result);
            CombinationAssert.RowEqual(expectedResult, result);
            CombinationAssert.CellEqual(expectedResult, result);
        }

        [Theory]
        [JsonFileData(".//Resources//" + TestConstants.TEST_FILE_GROUP_BY_ANY_WITHOUT_COMBINATIONS)]
        async public void GroupByAnyWithoutCombination(CombinationTest combinationTest, ExpectedResult expectedResult)
        {
            var result = await GroupBy(combinationTest, new List<LayoutType>());

            CombinationAssert.Null(result);
        }
    }
}
