using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByThreeService : CombinationGroupByTwoService
    {
        public CombinationGroupByThreeService(FeaturesContext context, IVersionProvider versionProvider, GroupProviderBuilder groupProviderBuilder)
            : base(context, versionProvider, groupProviderBuilder)
        {
        }

        protected override IGroupProvider GetGroupProvider(CombinationSearchDTO search, GroupProviderBuilder groupProviderBuilder)
        {
            var groupProvider = groupProviderBuilder
                .GroupByOne(search.RowLayout)
                .GroupByTwo(search.ColumnLayout)
                .GroupByThree(search.CellLayout)
                .Build();

            return groupProvider;
        }
    }
}
