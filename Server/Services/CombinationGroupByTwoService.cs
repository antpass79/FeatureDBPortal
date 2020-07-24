using FeatureDBPortal.Server.Builders;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByTwoService : CombinationGroupService
    {
        public CombinationGroupByTwoService(
            FeaturesContext context,
            IVersionProvider versionProvider,
            GroupProviderBuilder groupProviderBuilder)
            : base(context, versionProvider, groupProviderBuilder)
        {
        }

        protected override IGroupProvider BuildGroupProvider(CombinationSearchDTO search, GroupProviderBuilder groupProviderBuilder)
        {
            // If in output there are Model and Country:
            // - Foreach groups take ids (in case of model the firstGroup id is ModelId, the secondGroup id is CountryId)
            // - Build the default version with firstGroup.Id and secondGroup.Id
            // - Filter rules with the default version (see FilterNormalRules function in the base class)
            // NOTE: only if there isn't a version in the search

            return groupProviderBuilder
                .GroupByOne(search.RowLayout)
                .GroupByTwo(search.ColumnLayout)
                .Build();
        }
    }
}
