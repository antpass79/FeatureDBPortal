using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByTwoService : CombinationGroupService
    {
        public CombinationGroupByTwoService(DbContext context, IVersionProvider versionProvider)
            : base(context, versionProvider)
        {
        }

        async override protected Task<CombinationDTO> GroupNormalRules(CombinationSearchDTO search, IQueryable<NormalRule> normalRules, IEnumerable<LayoutType> groupBy)
        {
            var firstLayoutGroup = groupBy
                .ElementAt(0)
                .ToString();
            var secondLayoutGroup = groupBy
                .ElementAt(1)
                .ToString();

            var fakeIds = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup)
                .ToList()
                .Where(item => item.IsFake)
                .Select(item => item.Id)
                .ToList();

            var orderedSelectedRowField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup)
                .AsEnumerable()
                .Where(item => !item.IsFake)
                .Select(item => new QueryableCombination { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToList();

            var orderedSelectedColumnField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(secondLayoutGroup)
                .AsEnumerable()
                .Select(item => new QueryableCombination { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToList();

            // If in output there are Model and Country:
            // - Foreach groups take ids (in case of model the firstGroup id is ModelId, the secondGroup id is CountryId)
            // - Build the default version with firstGroup.Id and secondGroup.Id
            // - Filter rules with the default version (see FilterNormalRules function in the base class)
            // NOTE: only if there isn't a version in the search

            var groups = normalRules
                .GroupBy(PropertyExpressionBuilder.Build<NormalRule, int?>($"{firstLayoutGroup}Id").Compile())
                .ToList()
                .Where(item => item.Key.HasValue && !fakeIds.Contains(item.Key.Value))
                .Select(group => new
                {
                    RowId = group.Key,
                    RowName = orderedSelectedRowField.SingleOrDefault(item => item.Id == group.Key)?.Name,
                    Cells = group.Select(groupItem =>
                    {
                        var newCell = new CombinationCell
                        {
                            RowId = group.Key,
                            ColumnId = groupItem.GetPropertyIdByGroupName(secondLayoutGroup),
                            Name = orderedSelectedColumnField.SingleOrDefault(item => item.Id == groupItem.GetPropertyIdByGroupName(secondLayoutGroup))?.Name,
                            Available = group.All(item => (AllowMode)item.Allow != AllowMode.No),
                            Visible = group.All(item => (AllowMode)item.Allow == AllowMode.A)
                        };
                        newCell.AllowMode = Allower.GetMode(newCell.Visible.Value, newCell.Available.Value);

                        return newCell;
                    }).ToList()
                }).ToList();

            CombinationDictionary matrix = PrepareMatrix(orderedSelectedRowField, orderedSelectedColumnField);

            for (var x = 0; x < groups.Count; x++)
            {
                var rowA = groups[x];
                for (var y = 0; y < rowA.Cells.Count; y++)
                {
                    var columnA = rowA.Cells[y];

                    // ANTO why id can be null?
                    if (!columnA.RowId.HasValue || !columnA.ColumnId.HasValue)
                        continue;

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
                                Name = columnA.Name,
                                Available = columnA.Available,
                                Visible = columnA.Visible,
                                AllowMode = columnA.AllowMode
                            };
                        }
                        else
                        {
                            matrix[rowKey].Add(columnKey, new CombinationCell
                            {
                                RowId = rowKey,
                                ColumnId = columnKey,
                                Name = columnA.Name,
                                Available = columnA.Available,
                                Visible = columnA.Visible,
                                AllowMode = columnA.AllowMode
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
                            Name = columnA.Name,
                            Available = columnA.Available,
                            Visible = columnA.Visible,
                            AllowMode = columnA.AllowMode
                        };
                        matrix[rowKey] = newRow;
                    }
                }
            }

            var combination = new CombinationDTO
            {
                IntersectionTitle = $"{firstLayoutGroup} / {secondLayoutGroup}",
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
