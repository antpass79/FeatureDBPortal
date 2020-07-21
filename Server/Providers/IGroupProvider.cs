using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public interface IGroupProvider
    {
        string GroupName { get; }
        public IReadOnlyList<QueryableCombination> Rows { get; }
        public IReadOnlyList<QueryableCombination> Columns { get; }
        CombinationDTO Group(IQueryable<NormalRule> normalRules);
    }
}
