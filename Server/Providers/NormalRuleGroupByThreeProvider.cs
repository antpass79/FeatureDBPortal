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

        public CombinationDTO GroupFast(IQueryable<NormalRule> normalRules)
        {
            var combination = BuildCombination(Rows, Columns);
            combination.IntersectionTitle = GroupName;

            var groups = normalRules
                .GroupBy(_rowGroupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_rowGroupProperties.DiscardItemIds.Contains(item.Key.Value))
                .ToList();

            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];

                var row = combination.Rows[_rowIdToIndexMapper[group.Key]];

                foreach (var normalRule in group)
                {
                    var columnId = normalRule.GetPropertyIdByGroupNameId(_columnGroupProperties.NormalRulePropertyNameId);
                    if (!columnId.HasValue)
                        continue;

                    var cell = row.Cells[_columnIdToIndexMapper[columnId]];
                    cell.ColumnId = columnId;
                    cell.Available = group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No);
                    cell.Visible = group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A);
                    cell.AllowMode = (AllowModeDTO)Allower.GetMode(cell.Visible.Value, cell.Available.Value);
                    cell.Items = group.GroupBy(_cellGroupProperties.GroupExpression).Select(thirdGroup => new CellItemDTO
                    {
                        Name = _cellGroupProperties.GroupableItems.SingleOrDefault(cellItem => cellItem.Id == thirdGroup.Key)?.Name,
                        Allow = thirdGroup.All(i => i.Allow != 0)
                    })
                        .Where(item => item.Allow)
                        .ToList();
                }
            }

            return combination;
        }

        Dictionary<int?, int> _rowIdToIndexMapper = new Dictionary<int?, int>();
        Dictionary<int?, int> _columnIdToIndexMapper = new Dictionary<int?, int>();
        CombinationDTO BuildCombination(IReadOnlyList<QueryableCombination> rows, IReadOnlyList<QueryableCombination> columns)
        {
            var combination = new CombinationDTO();
            var newRows = new List<RowDTO>();

            var rowIndex = 0;
            foreach (var row in rows)
            {
                _rowIdToIndexMapper.Add(row.Id, rowIndex);

                var newRow = new RowDTO
                {
                    RowId = row.Id,
                    Title = new RowTitleDTO
                    {
                        Id = row.Id,
                        Name = row.Name
                    }
                };

                var newCells = new List<CellDTO>();
                foreach (var column in columns)
                {
                    var newCell = new CellDTO
                    {
                        RowId = row.Id,
                        ColumnId = column.Id,
                    };

                    newCells.Add(newCell);
                }
                newRow.Cells = newCells;

                newRows.Add(newRow);

                rowIndex++;
            }

            int columnIndex = 0;
            var newColumns = new List<ColumnDTO>();
            foreach (var column in columns)
            {
                _columnIdToIndexMapper.Add(column.Id, columnIndex);
                var newColumn = new ColumnDTO
                {
                    Id = column.Id,
                    Name = column.Name
                };

                newColumns.Add(newColumn);

                columnIndex++;
            }

            combination.Rows = newRows;
            combination.Columns = newColumns;

            return combination;
        }
    }
}
