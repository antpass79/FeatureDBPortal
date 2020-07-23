using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByOneService : CombinationGroupService
    {
        public CombinationGroupByOneService(FeaturesContext context, IVersionProvider versionProvider, GroupProviderBuilder groupProviderBuilder)
            : base(context, versionProvider, groupProviderBuilder)
        {
        }

        async override protected Task<CombinationDTO> BuildCombination(CombinationSearchDTO search, IList<NormalRule> normalRules, GroupProviderBuilder groupProviderBuilder)
        {
            var groupProvider = groupProviderBuilder
                .GroupByOne(search.RowLayout)
                .Build();

            // If in output there are Model or Country:
            // - Foreach group take id (in case of model is the ModelId)
            // - Build the default version with group.Id and search.CountryId
            // - Filter rules with the default version (see FilterNormalRules function in the base class)
            // NOTE: only if there isn't a version in the search

            // Maybe add firstLayoutGroup + "Id" == null
            var combination = groupProvider.Group(new GroupParameters { NormalRules = normalRules, ProbeId = search.ProbeId });
            return await Task.FromResult(combination);
        }
    }
}
