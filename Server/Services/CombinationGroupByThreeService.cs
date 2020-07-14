using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByThreeService : CombinationGroupByTwoService
    {
        public CombinationGroupByThreeService(DbContext context, IVersionProvider versionProvider)
            : base(context, versionProvider)
        {
        }

        protected override INormalRuleGroupProvider GetGroupProvider(CombinationSearchDTO search)
        {
            var groupProvider = new NormalRuleGroupProviderBuilder(Context)
                .GroupByOne(search.RowLayout)
                .GroupByTwo(search.ColumnLayout)
                .GroupByThree(search.CellLayout)
                .Build();

            return groupProvider;
        }

        protected override void FillMatrix(CombinationMatrix matrix, IReadOnlyList<RowDTO> groups)
        {
            for (var x = 0; x < groups.Count; x++)
            {
                var row = groups[x];
                for (var y = 0; y < row.Cells.Count; y++)
                {
                    var column = row.Cells[y];

                    // ANTO why id can be null?
                    if (!column.RowId.HasValue || !column.ColumnId.HasValue)
                        continue;

                    var rowKey = column.RowId.HasValue ? column.RowId : -1;
                    var columnKey = column.ColumnId.HasValue ? column.ColumnId : -1;

                    if (matrix.ContainsKey(rowKey))
                    {
                        var selectedRow = matrix[rowKey];

                        if (selectedRow.ContainsKey(columnKey))
                        {
                            matrix[rowKey][columnKey] = new CombinationCell
                            {
                                RowId = rowKey,
                                ColumnId = columnKey,
                                Available = column.Available,
                                Visible = column.Visible,
                                AllowMode = (AllowMode)column.AllowMode,
                                    Items = column.Items?
                                    .Where(cellItem => cellItem.Allow)
                                    .Select(cellItem => new CombinationItem
                                    {
                                        RowId = rowKey,
                                        ColumnId = columnKey,
                                        Name = cellItem.Name
                                    })
                            };
                        }
                        else
                        {
                            matrix[rowKey].Add(columnKey, new CombinationCell
                            {
                                RowId = rowKey,
                                ColumnId = columnKey,
                                Available = column.Available,
                                Visible = column.Visible,
                                AllowMode = (AllowMode)column.AllowMode,
                                Items = column.Items?
                                    .Where(cellItem => cellItem.Allow)
                                    .Select(cellItem => new CombinationItem
                                    {
                                        RowId = rowKey,
                                        ColumnId = columnKey,
                                        Name = cellItem.Name
                                    })
                            });
                        }
                    }
                    else
                    {
                        var newRow = new CombinationRow(1);
                        newRow[columnKey] = new CombinationCell()
                        {
                            RowId = rowKey,
                            ColumnId = columnKey,
                            Available = column.Available,
                            Visible = column.Visible,
                            AllowMode = (AllowMode)column.AllowMode,
                            Items = column.Items?
                                .Where(cellItem => cellItem.Allow)
                                .Select(cellItem => new CombinationItem
                                {
                                    RowId = rowKey,
                                    ColumnId = columnKey,
                                    Name = cellItem.Name
                                })
                        };
                        matrix[rowKey] = newRow;
                    }
                }
            }
        }
    }
}
