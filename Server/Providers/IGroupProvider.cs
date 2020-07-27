using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Providers
{
    public interface IGroupProvider
    {
        string GroupName { get; }
        public IReadOnlyList<QueryableEntity> Rows { get; }
        public IReadOnlyList<QueryableEntity> Columns { get; }
        CombinationDTO Group(IList<NormalRule> normalRules, GroupParameters parameters);
    }
}
