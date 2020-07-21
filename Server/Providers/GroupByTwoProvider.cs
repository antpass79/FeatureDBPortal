using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByTwoProvider : IGroupProvider
    {
        private readonly GroupProperties _rowGroupProperties;
        private readonly GroupProperties _columnGroupProperties;

        public GroupByTwoProvider(
            GroupProperties rowGroupProperties,
            GroupProperties columnGroupProperties)
        {
            _rowGroupProperties = rowGroupProperties;
            _columnGroupProperties = columnGroupProperties;

            _rows = _rowGroupProperties.GroupableItems.Values.ToList();
            _columns = _columnGroupProperties.GroupableItems.Values.ToList();
        }

        public string GroupName => $"{_rowGroupProperties.LayoutType} / {_columnGroupProperties.LayoutType}";

        IReadOnlyList<QueryableCombination> _rows;
        public IReadOnlyList<QueryableCombination> Rows => _rows;
        IReadOnlyList<QueryableCombination> _columns;
        public IReadOnlyList<QueryableCombination> Columns => _columns;

        public CombinationDTO Group(IList<NormalRule> normalRules)
        {
            var combination = BuildCombination(Rows, Columns);
            combination.IntersectionTitle = GroupName;

            var groups = normalRules
                .GroupBy(_rowGroupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_rowGroupProperties.DiscardItemIds.Contains(item.Key.Value))
                .ToList();

            Parallel.For(0, groups.Count, (i) =>
            {
                var group = groups[i];
                var row = combination.Rows[_rowIdToIndexMapper[group.Key]];

                Parallel.ForEach(group, (normalRule) =>
                {
                    var columnId = normalRule.GetPropertyIdByGroupNameId(_columnGroupProperties.NormalRulePropertyNameId);
                    if (columnId.HasValue)
                    {
                        var cell = row.Cells[_columnIdToIndexMapper[columnId]];

                        cell.Available = group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No);
                        cell.Visible = group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A);
                        cell.AllowMode = (AllowModeDTO)Allower.GetMode(cell.Visible.Value, cell.Available.Value);
                    }
                });
            });

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
