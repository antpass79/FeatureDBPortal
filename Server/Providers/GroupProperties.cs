using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupProperties
    {
        public GroupProperties(LayoutTypeDTO layoutType, List<QueryableCombination> groupableItems, IList<int> discardItemIds)
        {
            LayoutType = layoutType;

            TableName = LayoutType.ToNormalRulePropertyName();
            NormalRulePropertyName = LayoutType.ToNormalRulePropertyName();
            NormalRulePropertyNameId = LayoutType.ToNormalRulePropertyNameId();

            GroupExpression = PropertyExpressionBuilder.Build<NormalRule, int?>(NormalRulePropertyNameId).Compile();

            GroupableItems = groupableItems;
            DiscardItemIds = discardItemIds;
        }

        public LayoutTypeDTO LayoutType { get; }

        public string TableName { get; }
        public string NormalRulePropertyName { get; }
        public string NormalRulePropertyNameId { get; }

        public List<QueryableCombination> GroupableItems { get; }
        public IList<int> DiscardItemIds { get; }

        public Func<NormalRule, int?> GroupExpression { get; }
    }
}
