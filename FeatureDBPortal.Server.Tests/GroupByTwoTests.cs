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
    public class GroupByTwoTests : BaseGroupByTests
    {
        public GroupByTwoTests()
        {
        }

        protected override ICombinationGroupService BuildCombinationGroupService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<FeaturesContext>();
            optionsBuilder.UseSqlServer("Server=PC\\SQLExpress;Database=Features;Trusted_Connection=True;");
            var context = new FeaturesContext(optionsBuilder.Options);

            return new CombinationGroupByTwoService(context, new VersionProvider(context), new GroupProviderBuilder(context));
        }

        [Theory]
        [JsonFileData(".//Resources//" + TestConstants.TEST_FILE_GROUP_BY_TWO_WITH_COMBINATIONS)]
        async public void GroupByTwoWithCombination(CombinationTest combinationTest, ExpectedResult expectedResult)
        {
            var result = await GroupBy(combinationTest, new List<LayoutType>()
            {
                combinationTest.FirstGroup.Value,
                combinationTest.SecondGroup.Value
            });

            CombinationAssert.HeaderEqual(expectedResult, result);
            CombinationAssert.RowEqual(expectedResult, result);
            CombinationAssert.CellEqual(expectedResult, result);
        }

        //[Theory]
        //[JsonFileData(".//Resources//" + TestConstants.TEST_FILE_GROUP_BY_ONE_WITHOUT_COMBINATIONS)]
        async public void GroupByTwoWithoutCombination(CombinationTest combinationTest, ExpectedResult expectedResult)
        {
            var result = await GroupBy(combinationTest, new List<LayoutType>() { combinationTest.FirstGroup.Value });

            CombinationAssert.Empty(result);
        }
    }
}
