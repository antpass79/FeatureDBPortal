using FeatureDBPortal.Server.Builders;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByAnyService : CombinationGroupService
    {
        public CombinationGroupByAnyService(
            FeaturesContext context,
            IVersionProvider versionProvider,
            GroupProviderBuilder groupProviderBuilder)
            : base(context, versionProvider, groupProviderBuilder)
        {
        }

        protected override IGroupProvider BuildGroupProvider(CombinationSearchDTO search, GroupProviderBuilder groupProviderBuilder)
        {
            return groupProviderBuilder
                .Build();
        }
    }
}
