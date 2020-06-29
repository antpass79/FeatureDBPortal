using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByThreeService : CombinationGroupService
    {
        public CombinationGroupByThreeService(DbContext context)
            : base(context)
        {
        }

        async override public Task<CombinationDTO> Combine(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
        {
            var firstLayoutGroup = groupBy.ElementAt(0);
            var secondLayoutGroup = groupBy.ElementAt(1);
            var thirdLayoutGroup = groupBy.ElementAt(2);

            var firstPropertyNameId = firstLayoutGroup + "Id";
            var secondPropertyNameId = secondLayoutGroup + "Id";
            var thirdPropertyNameId = thirdLayoutGroup + "Id";

            var selectedRowField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup.ToString());
            var selectedColumnField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(secondLayoutGroup.ToString());
            var selectedCellField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(thirdLayoutGroup.ToString());

            IQueryable<NormalRule> normalRules = FilterNormalRules(search);

            var firstGroupExpression = PropertyExpressionBuilder.Build<NormalRule, int?>(firstPropertyNameId).Compile();
            var thirdGroupExpression = PropertyExpressionBuilder.Build<NormalRule, int?>(thirdPropertyNameId).Compile();

            Func<NormalRule, string, int?> getPropertyValue = (NormalRule nr, string propertyName) =>
            {
                var result = propertyName switch
                {
                    "ApplicationId" => nr.ApplicationId,
                    "ProbeId" => nr.ProbeId,
                    "OptionId" => nr.OptionId,
                    _ => throw new NotSupportedException()
                };

                return result;
            };

            var groups = normalRules
                .GroupBy(firstGroupExpression)
                .Select(group => new
                {
                    RowId = group.Key,
                    RowName = selectedRowField.SingleOrDefault(item => item.Id == group.Key)?.Name,
                    Cells = group.Select(groupItem => new
                    {
                        RowId = group.Key,
                        ColumnId = getPropertyValue(groupItem, secondPropertyNameId),
                        //ColumnId = groupItem.GetPropertyValue<int?>(secondPropertyNameId),
                        Available = group.All(item => (AllowMode)item.Allow != AllowMode.No),
                        Visible = group.All(item => (AllowMode)item.Allow == AllowMode.A),
                        AllowMode = Allower.GetMode(group.All(item => (AllowMode)item.Allow == AllowMode.A), group.All(item => (AllowMode)item.Allow != AllowMode.No)),
                        Items = group.GroupBy(thirdGroupExpression).Select(thirdGroup => new
                        {
                            ItemName = selectedCellField.SingleOrDefault(cellItem => cellItem.Id == thirdGroup.Key)?.Name,
                            Allow = thirdGroup.All(i => i.Allow != 0)
                        })
                    }).ToList()
                }).ToList();

            var orderedSelectedRowField = selectedRowField
                .ToList()
                .OrderBy(item => item.Name)
                .ToList();
            var orderedSelectedColumnField = selectedColumnField
                .ToList()
                .OrderBy(item => item.Name)
                .ToList();

            CombinationDictionary matrix = PrepareMatrix(orderedSelectedRowField, orderedSelectedColumnField);

            for (var x = 0; x < groups.Count; x++)
            {
                var rowA = groups[x];
                for (var y = 0; y < rowA.Cells.Count; y++)
                {
                    var columnA = rowA.Cells[y];

                    var rowKey = columnA.RowId.HasValue ? columnA.RowId : -1;
                    var columnKey = columnA.ColumnId.HasValue ? columnA.ColumnId : -1;

                    if (matrix.ContainsKey(rowKey))
                    {
                        var selectedRow = matrix[rowKey];

                        if (selectedRow.ContainsKey(columnKey))
                        {
                            matrix[rowKey][columnKey] = new CombinationCell
                            {
                                RowId = rowKey,
                                ColumnId = columnKey,
                                Available = columnA.Available,
                                Visible = columnA.Visible,
                                AllowMode = columnA.AllowMode,
                                Items = columnA.Items?
                                    .Where(cellItem => cellItem.Allow)
                                .Select(cellItem => new CombinationItem
                                {
                                    RowId = rowKey,
                                    ColumnId = columnKey,
                                    Name = cellItem.ItemName
                                })
                            };
                        }
                        else
                        {
                            matrix[rowKey].Add(columnKey, new CombinationCell
                            {
                                RowId = rowKey,
                                ColumnId = columnKey,
                                Available = columnA.Available,
                                Visible = columnA.Visible,
                                AllowMode = columnA.AllowMode,
                                Items = columnA.Items?
                                    .Where(cellItem => cellItem.Allow)
                                .Select(cellItem => new CombinationItem
                                {
                                    RowId = rowKey,
                                    ColumnId = columnKey,
                                    Name = cellItem.ItemName
                                })
                            });
                        }
                    }
                    else
                    {
                        var newRow = new RowDictionary(1);
                        newRow[columnKey] = new CombinationCell()
                        {
                            RowId = rowKey,
                            ColumnId = columnKey,
                            Available = columnA.Available,
                            Visible = columnA.Visible,
                            AllowMode = columnA.AllowMode,
                            Items = columnA.Items?
                                .Where(cellItem => cellItem.Allow)
                                .Select(cellItem => new CombinationItem
                                {
                                    RowId = rowKey,
                                    ColumnId = columnKey,
                                    Name = cellItem.ItemName
                                })
                        };
                        matrix[rowKey] = newRow;
                    }
                }
            }
            
            var firstHeaderItem = new List<ColumnTitleDTO> { new ColumnTitleDTO
            {
                Name = $"{firstLayoutGroup} - {secondLayoutGroup} - {thirdLayoutGroup}"
            } };

            var combination = new CombinationDTO
            {
                Headers = firstHeaderItem.Union(orderedSelectedColumnField.Select(item => new ColumnTitleDTO { Id = item.Id, Name = item.Name })),
                Rows = matrix.ToRows()
            };

            return await Task.FromResult(combination);
        }

        private static CombinationDictionary PrepareMatrix(List<IQueryableCombination> orderedSelectedRowField, List<IQueryableCombination> orderedSelectedColumnField)
        {
            var matrix = new CombinationDictionary(orderedSelectedRowField.Count);

            orderedSelectedRowField
                .ForEach(rowItem =>
                {
                    var row = new RowDictionary(orderedSelectedColumnField.Count);
                    row.Name = rowItem.Name;
                    orderedSelectedColumnField
                    .ForEach(columnItem =>
                    {
                        row[columnItem.Id] = new CombinationCell
                        {
                            RowId = rowItem.Id,
                            ColumnId = columnItem.Id
                        };
                    });
                    matrix[rowItem.Id] = row;
                });
            return matrix;
        }
    }
}
