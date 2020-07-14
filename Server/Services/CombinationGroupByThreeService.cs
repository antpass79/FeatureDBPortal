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
    public class CombinationGroupByThreeService : CombinationGroupByTwoService
    {
        public CombinationGroupByThreeService(DbContext context, IVersionProvider versionProvider)
            : base(context, versionProvider)
        {
        }

        protected override INormalRuleGroupProvider GetGroupProvider(CombinationSearchDTO search)
        {
            var groupProvider = new NormalRuleGroupProviderBuilder(Context)
                .GroupByOne(search.RowLayout)
                .GroupByTwo(search.ColumnLayout)
                .GroupByThree(search.CellLayout)
                .Build();

            return groupProvider;
        }
    }
}
