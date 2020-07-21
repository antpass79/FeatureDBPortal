using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByAnyProvider : IGroupProvider
    {
        public GroupByAnyProvider()
        {
        }

        public string GroupName => string.Empty;

        public IReadOnlyList<QueryableCombination> Rows => throw new NotImplementedException();
        public IReadOnlyList<QueryableCombination> Columns => throw new NotSupportedException();

        public CombinationDTO Group(IList<NormalRule> normalRules)
        {
            throw new NotImplementedException();
        }
    }
}
