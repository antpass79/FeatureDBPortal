using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class NormalRuleGroupByOneProvider : INormalRuleGroupProvider
    {
        private readonly NormalRuleGroupProperties _groupProperties;
        public NormalRuleGroupByOneProvider(NormalRuleGroupProperties groupProperties)
        {
            _groupProperties = groupProperties;
        }

        public string GroupName => _groupProperties.NormalRulePropertyName;

        public IReadOnlyList<QueryableCombination> Rows => _groupProperties.GroupableItems;
        public IReadOnlyList<QueryableCombination> Columns => throw new NotSupportedException();

        public IReadOnlyList<RowDTO> Group(IQueryable<NormalRule> normalRules)
        {
            // Maybe add firstLayoutGroup + "Id" == null
            var groups = normalRules
                .GroupBy(_groupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_groupProperties.DiscardItemIds.Contains(item.Key.Value))
                .Select(group => new RowDTO
                {
                    RowId = group.Key,
                    Title = new RowTitleDTO
                    {
                        Id = group.Key,
                        Name = _groupProperties.GroupableItems.SingleOrDefault(item => item.Id == group.Key)?.Name
                    },
                    Cells = new List<CellDTO>
                    {
                        new CellDTO
                        {
                            RowId = group.Key,
                            ColumnId = -1,
                            Available = group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No),
                            Visible = group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A),
                            AllowMode = (AllowModeDTO)Allower.GetMode(group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A), group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No))
                        }
                    }
                    }).ToList()
                .OrderBy(item => item.Title.Name)
                .ToList();

            return groups;
        }
    }
}
