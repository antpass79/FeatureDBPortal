using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByAnyService : CombinationGroupService
    {
        private readonly IAllowModeProvider _allowModeProvider;
        public CombinationGroupByAnyService(
            FeaturesContext context,
            IVersionProvider versionProvider,
            GroupProviderBuilder groupProviderBuilder,
            IAllowModeProvider allowModeProvider)
            : base(context, versionProvider, groupProviderBuilder)
        {
            _allowModeProvider = allowModeProvider;
        }

        async override protected Task<CombinationDTO> BuildCombination(CombinationSearchDTO search, IList<NormalRule> normalRules, GroupProviderBuilder groupProviderBuilder)
        {
            var groupProvider = groupProviderBuilder
                .Build();

            var combination = groupProvider.Group(new GroupParameters { NormalRules = normalRules, ProbeId = search.ProbeId });
            return await Task.FromResult(combination);
        }
    }
}
