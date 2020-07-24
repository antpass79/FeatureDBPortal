using FeatureDBPortal.Server.Builders;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByOneService : CombinationGroupService
    {
        public CombinationGroupByOneService(
            FeaturesContext context,
            IVersionProvider versionProvider,
            GroupProviderBuilder groupProviderBuilder)
            : base(context, versionProvider, groupProviderBuilder)
        {
        }

        protected override IGroupProvider BuildGroupProvider(CombinationSearchDTO search, GroupProviderBuilder groupProviderBuilder)
        {
            // If in output there are Model or Country:
            // - Foreach group take id (in case of model is the ModelId)
            // - Build the default version with group.Id and search.CountryId
            // - Filter rules with the default version (see FilterNormalRules function in the base class)
            // NOTE: only if there isn't a version in the search

            // Maybe add firstLayoutGroup + "Id" == null

            return groupProviderBuilder
                .GroupByOne(search.RowLayout)
                .Build();
        }
    }
}
