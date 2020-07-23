using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByThreeProvider : IGroupProvider
    {
        private readonly GroupProperties _rowGroupProperties;
        private readonly GroupProperties _columnGroupProperties;
        private readonly GroupProperties _cellGroupProperties;
        private readonly IAllowModeProvider _allowModeProvider;

        public GroupByThreeProvider(
            GroupProperties rowGroupProperties,
            GroupProperties columnGroupProperties,
            GroupProperties cellGroupProperties,
            IAllowModeProvider allowModeProvider)
        {
            _rowGroupProperties = rowGroupProperties;
            _columnGroupProperties = columnGroupProperties;
            _cellGroupProperties = cellGroupProperties;

            _rows = _rowGroupProperties.GroupableItems.Values.ToList();
            _columns = _columnGroupProperties.GroupableItems.Values.ToList();

            _allowModeProvider = allowModeProvider;
        }

        public string GroupName => $"{_rowGroupProperties.LayoutType} / {_columnGroupProperties.LayoutType} / {_cellGroupProperties.LayoutType}";

        IReadOnlyList<QueryableEntity> _rows;
        public IReadOnlyList<QueryableEntity> Rows => _rows;
        IReadOnlyList<QueryableEntity> _columns;
        public IReadOnlyList<QueryableEntity> Columns => _columns;


        public CombinationDTO Group(GroupParameters parameters)
        {
            var combination = BuildCombination(Rows, Columns);
            combination.IntersectionTitle = GroupName;

            var groups = parameters.NormalRules
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

                        var allowModeProperties = _allowModeProvider.Properties(group, parameters.ProbeId);
                        cell.Visible = allowModeProperties.Visible;
                        cell.Available = allowModeProperties.Available;
                        cell.AllowMode = allowModeProperties.AllowMode;

                        cell.Items = group.GroupBy(_cellGroupProperties.GroupExpression).Select(thirdGroup => new CellItemDTO
                        {
                            Name = thirdGroup.Key.HasValue && _cellGroupProperties.GroupableItems.ContainsKey(thirdGroup.Key.Value) ? _cellGroupProperties.GroupableItems[thirdGroup.Key.Value].Name : string.Empty,
                            Allow = thirdGroup.All(i => i.Allow != 0)
                        })
                            .Where(item => item.Allow)
                            .ToList();
                    }
                });
            });

            return combination;
        }

        Dictionary<int?, int> _rowIdToIndexMapper = new Dictionary<int?, int>();
        Dictionary<int?, int> _columnIdToIndexMapper = new Dictionary<int?, int>();
        CombinationDTO BuildCombination(IReadOnlyList<QueryableEntity> rows, IReadOnlyList<QueryableEntity> columns)
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
