using FeatureDBPortal.Server.Builders;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByThreeService : CombinationGroupService
    {
        public CombinationGroupByThreeService(
            FeaturesContext context,
            IVersionProvider versionProvider,
            GroupProviderBuilder groupProviderBuilder)
            : base(context, versionProvider, groupProviderBuilder)
        {
        }

        protected override IGroupProvider BuildGroupProvider(CombinationSearchDTO search, GroupProviderBuilder groupProviderBuilder)
        {
            return groupProviderBuilder
                .GroupByOne(search.RowLayout)
                .GroupByTwo(search.ColumnLayout)
                .GroupByThree(search.CellLayout)
                .Build();
        }
    }
}
