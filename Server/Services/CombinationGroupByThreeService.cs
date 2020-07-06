﻿using FeatureDBPortal.Server.Data.Models.RD;
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

        async override protected Task<CombinationDTO> GroupNormalRules(IQueryable<NormalRule> normalRules, IEnumerable<LayoutType> groupBy)
        {
            var firstLayoutGroup = groupBy.ElementAt(0).ToString();
            var secondLayoutGroup = groupBy.ElementAt(1).ToString();
            var thirdLayoutGroup = groupBy.ElementAt(2).ToString();

            var orderedSelectedRowField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup)
                .AsEnumerable()
                .Select(item => new QueryableCombination { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToList();
            var orderedSelectedColumnField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(secondLayoutGroup)
                .AsEnumerable()
                .Select(item => new QueryableCombination { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToList();
            var orderedSelectedCellField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(thirdLayoutGroup)
                .AsEnumerable()
                .Select(item => new QueryableCombination { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToList();

            var firstGroupExpression = PropertyExpressionBuilder.Build<NormalRule, int?>($"{firstLayoutGroup}Id").Compile();
            var thirdGroupExpression = PropertyExpressionBuilder.Build<NormalRule, int?>($"{thirdLayoutGroup}Id").Compile();

            var groups = normalRules
                .GroupBy(firstGroupExpression)
                .Select(group => new
                {
                    RowId = group.Key,
                    RowName = orderedSelectedRowField.SingleOrDefault(item => item.Id == group.Key)?.Name,
                    Cells = group.Select(groupItem => new
                    {
                        RowId = group.Key,
                        ColumnId = groupItem.GetPropertyIdByGroupName(secondLayoutGroup),
                        Name = orderedSelectedColumnField.SingleOrDefault(item => item.Id == groupItem.GetPropertyIdByGroupName(secondLayoutGroup))?.Name,
                        Available = group.All(item => (AllowMode)item.Allow != AllowMode.No),
                        Visible = group.All(item => (AllowMode)item.Allow == AllowMode.A),
                        AllowMode = Allower.GetMode(group.All(item => (AllowMode)item.Allow == AllowMode.A), group.All(item => (AllowMode)item.Allow != AllowMode.No)),
                        Items = group.GroupBy(thirdGroupExpression).Select(thirdGroup => new
                        {
                            ItemName = orderedSelectedCellField.SingleOrDefault(cellItem => cellItem.Id == thirdGroup.Key)?.Name,
                            Allow = thirdGroup.All(i => i.Allow != 0)
                        })
                    }).ToList()
                }).ToList();

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
            
            var combination = new CombinationDTO
            {
                IntersectionTitle = $"{firstLayoutGroup} - {secondLayoutGroup} - {thirdLayoutGroup}",
                Columns = orderedSelectedColumnField.Select(item => new ColumnDTO { Id = item.Id, Name = item.Name }),
                Rows = matrix.ToRows()
            };

            return await Task.FromResult(combination);
        }

        private static CombinationDictionary PrepareMatrix(List<QueryableCombination> orderedSelectedRowField, List<QueryableCombination> orderedSelectedColumnField)
        {
            var matrix = new CombinationDictionary(orderedSelectedRowField.Count);

            orderedSelectedRowField
                .ForEach(rowItem =>
                {
                    var row = new RowDictionary(orderedSelectedColumnField.Count)
                    {
                        RowId = rowItem.Id,
                        Name = rowItem.Name
                    };
                    orderedSelectedColumnField
                    .ForEach(columnItem =>
                    {
                        row[columnItem.Id] = new CombinationCell
                        {
                            RowId = rowItem.Id,
                            ColumnId = columnItem.Id,
                            Name = columnItem.Name
                        };
                    });
                    matrix[rowItem.Id] = row;
                });
            return matrix;
        }
    }
}
