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
        public CombinationGroupByTwoService(FeaturesContext context, IVersionProvider versionProvider, GroupProviderBuilder groupProviderBuilder)
            : base(context, versionProvider, groupProviderBuilder)
        {
        }

        async override sealed protected Task<CombinationDTO> BuildCombination(CombinationSearchDTO search, IList<NormalRule> normalRules, GroupProviderBuilder groupProviderBuilder)
        {
            var groupProvider = GetGroupProvider(search, groupProviderBuilder);

            var combination = groupProvider.Group(new GroupParameters { NormalRules = normalRules, ProbeId = search.ProbeId });
            // If in output there are Model and Country:
            // - Foreach groups take ids (in case of model the firstGroup id is ModelId, the secondGroup id is CountryId)
            // - Build the default version with firstGroup.Id and secondGroup.Id
            // - Filter rules with the default version (see FilterNormalRules function in the base class)
            // NOTE: only if there isn't a version in the search

            return await Task.FromResult(combination);
        }

        protected virtual IGroupProvider GetGroupProvider(CombinationSearchDTO search, GroupProviderBuilder groupProviderBuilder)
        {
            var groupProvider = groupProviderBuilder
                .GroupByOne(search.RowLayout)
                .GroupByTwo(search.ColumnLayout)
                .Build();

            return groupProvider;
        }

        protected void FillMatrix(CombinationMatrix matrix, IReadOnlyList<RowDTO> groups)
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

                    var newCell = BuildCombinationCell(column);

                    if (matrix.ContainsKey(rowKey))
                    {
                        var selectedRow = matrix[rowKey];

                        if (selectedRow.ContainsKey(columnKey))
                        {
                            matrix[rowKey][columnKey] = newCell;
                        }
                        else
                        {
                            matrix[rowKey].Add(columnKey, newCell);
                        }
                    }
                    else
                    {
                        var newRow = new CombinationRow(1);
                        newRow[columnKey] = newCell;
                        matrix[rowKey] = newRow;
                    }
                }
            }
        }

        private CombinationCell BuildCombinationCell(CellDTO cell)
        {
            return new CombinationCell
            {
                RowId = cell.RowId,
                ColumnId = cell.ColumnId,
                Available = cell.Available,
                Visible = cell.Visible,
                AllowMode = cell.AllowMode,
                Items = cell.Items?
                                   .Where(cellItem => cellItem.Allow)
                                   .Select(cellItem => new CombinationItem
                                   {
                                       RowId = cell.RowId,
                                       ColumnId = cell.ColumnId,
                                       Name = cellItem.Name
                                   })
            };
        }
    }
}
