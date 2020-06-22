using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByTwoService : CombinationGroupService
    {
        public CombinationGroupByTwoService(DbContext context)
            : base(context)
        {
        }

        async override public Task<CombinationDTO> Combine(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
        {
            var firstLayoutGroup = groupBy
                .ElementAt(0)
                .ToString();
            var secondLayoutGroup = groupBy
                .ElementAt(1)
                .ToString();

            var selectedRowField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup);
            var selectedColumnField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(secondLayoutGroup);

            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

            var groups = normalRules
                .GroupBy(PropertyExpressionBuilder.Build<NormalRule, int?>(firstLayoutGroup + "Id").Compile())
                .Select(group => new
                {
                    RowId = group.Key,
                    RowName = selectedRowField.SingleOrDefault(item => item.Id == group.Key)?.Name,
                    Combinations = group.Select(groupItem => new
                    {
                        RowId = group.Key,
                        Allow = group.All(item => item.Allow != 0),
                        ColumnId = groupItem.GetPropertyValue<int?>(secondLayoutGroup + "Id")
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
                for (var y = 0; y < rowA.Combinations.Count; y++)
                {
                    var columnA = rowA.Combinations[y];

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
                                Allow = columnA.Allow
                            };
                        }
                        else
                        {
                            matrix[rowKey].Add(columnKey, new CombinationCell
                            {
                                RowId = rowKey,
                                ColumnId = columnKey,
                                Allow = columnA.Allow
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
                            Allow = columnA.Allow
                        };
                        matrix[rowKey] = newRow;
                    }
                }
            }

            var firstHeaderItem = new List<ColumnTitleDTO> { new ColumnTitleDTO
            {
                Name = $"{firstLayoutGroup} / {secondLayoutGroup}"
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
