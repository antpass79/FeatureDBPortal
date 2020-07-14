using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class NormalRuleGroupByThreeProvider : INormalRuleGroupProvider
    {
        private readonly NormalRuleGroupProperties _rowGroupProperties;
        private readonly NormalRuleGroupProperties _columnGroupProperties;
        private readonly NormalRuleGroupProperties _cellGroupProperties;

        public NormalRuleGroupByThreeProvider(
            NormalRuleGroupProperties rowGroupProperties,
            NormalRuleGroupProperties columnGroupProperties,
            NormalRuleGroupProperties cellGroupProperties)
        {
            _rowGroupProperties = rowGroupProperties;
            _columnGroupProperties = columnGroupProperties;
            _cellGroupProperties = cellGroupProperties;
        }

        public string GroupName => $"{_rowGroupProperties.LayoutType} / {_columnGroupProperties.LayoutType} / {_cellGroupProperties.LayoutType}";

        public IReadOnlyList<QueryableCombination> Rows => _rowGroupProperties.GroupableItems;
        public IReadOnlyList<QueryableCombination> Columns => _columnGroupProperties.GroupableItems;

        public IReadOnlyList<RowDTO> Group(IQueryable<NormalRule> normalRules)
        {
            var groups = normalRules
                .GroupBy(_rowGroupProperties.GroupExpression)
                .Select(group => new RowDTO
                {
                    RowId = group.Key,
                    Title = new RowTitleDTO
                    {
                        Id = group.Key,
                        Name = _rowGroupProperties.GroupableItems.SingleOrDefault(item => item.Id == group.Key)?.Name
                    },
                    Cells = group.Select(groupItem => new CellDTO
                    {
                        RowId = group.Key,
                        ColumnId = groupItem.GetPropertyIdByGroupNameId(_columnGroupProperties.NormalRulePropertyNameId),
                        Available = group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No),

                        Visible = group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A),
                        AllowMode = (AllowModeDTO)Allower.GetMode(group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A), group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No)),
                        Items = group.GroupBy(_cellGroupProperties.GroupExpression).Select(thirdGroup => new CellItemDTO
                        {
                            Name = _cellGroupProperties.GroupableItems.SingleOrDefault(cellItem => cellItem.Id == thirdGroup.Key)?.Name,
                            Allow = thirdGroup.All(i => i.Allow != 0)
                        }).ToList()
                    }).ToList()
                }).ToList();

            return groups;
        }
    }
}
