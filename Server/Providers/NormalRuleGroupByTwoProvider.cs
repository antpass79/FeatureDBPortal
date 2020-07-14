using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class NormalRuleGroupByTwoProvider : INormalRuleGroupProvider
    {
        private readonly NormalRuleGroupProperties _rowGroupProperties;
        private readonly NormalRuleGroupProperties _columnGroupProperties;

        public NormalRuleGroupByTwoProvider(
            NormalRuleGroupProperties rowGroupProperties,
            NormalRuleGroupProperties columnGroupProperties)
        {
            _rowGroupProperties = rowGroupProperties;
            _columnGroupProperties = columnGroupProperties;
        }

        public string GroupName => $"{_rowGroupProperties.LayoutType} / {_columnGroupProperties.LayoutType}";

        public IReadOnlyList<QueryableCombination> Rows => _rowGroupProperties.GroupableItems;
        public IReadOnlyList<QueryableCombination> Columns => _columnGroupProperties.GroupableItems;

        public IReadOnlyList<RowDTO> Group(IQueryable<NormalRule> normalRules)
        {
            var groups = normalRules
                .GroupBy(_rowGroupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_rowGroupProperties.DiscardItemIds.Contains(item.Key.Value))
                .Select(group => new RowDTO
                {
                    RowId = group.Key,
                    Title = new RowTitleDTO
                    {
                        Id = group.Key,
                        Name = _rowGroupProperties.GroupableItems.SingleOrDefault(item => item.Id == group.Key)?.Name
                    },
                    Cells = group.Select(normalRule =>
                    {
                        var newCell = new CellDTO
                        {
                            RowId = group.Key,
                            ColumnId = normalRule.GetPropertyIdByGroupNameId(_columnGroupProperties.NormalRulePropertyNameId),
                            Available = group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No),
                            Visible = group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A)
                        };
                        newCell.AllowMode = (AllowModeDTO)Allower.GetMode(newCell.Visible.Value, newCell.Available.Value);

                        return newCell;
                    }).ToList()
                }).ToList();

            return groups;
        }
    }
}
