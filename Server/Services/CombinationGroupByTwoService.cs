﻿using FeatureDBPortal.Server.Data.Models.RD;
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

        async override sealed protected Task<CombinationDTO> BuildCombination(CombinationSearchDTO search, IQueryable<NormalRule> normalRules)
        {
            var groupProvider = GetGroupProvider(search);

            // If in output there are Model and Country:
            // - Foreach groups take ids (in case of model the firstGroup id is ModelId, the secondGroup id is CountryId)
            // - Build the default version with firstGroup.Id and secondGroup.Id
            // - Filter rules with the default version (see FilterNormalRules function in the base class)
            // NOTE: only if there isn't a version in the search

            var groups = groupProvider
                .Group(normalRules);

            var matrix = BuildMatrix(groupProvider.Rows, groupProvider.Columns);
            FillMatrix(matrix, groups);
            var combination = MapToCombination(matrix, groupProvider);

            return await Task.FromResult(combination);
        }

        protected virtual INormalRuleGroupProvider GetGroupProvider(CombinationSearchDTO search)
        {
            var groupProvider = new NormalRuleGroupProviderBuilder(Context)
                .GroupByOne(search.RowLayout)
                .GroupByTwo(search.ColumnLayout)
                .Build();

            return groupProvider;
        }

        protected virtual void FillMatrix(CombinationMatrix matrix, IReadOnlyList<RowDTO> groups)
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
                                AllowMode = (AllowMode)column.AllowMode
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
                                AllowMode = (AllowMode)column.AllowMode
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
                            AllowMode = (AllowMode)column.AllowMode
                        };
                        matrix[rowKey] = newRow;
                    }
                }
            }
        }

        private CombinationMatrix BuildMatrix(IReadOnlyList<QueryableCombination> rows, IReadOnlyList<QueryableCombination> columns)
        {
            var matrix = new CombinationMatrix(rows.Count);

            foreach (var row in rows)
            {
                var newRow = new CombinationRow(columns.Count)
                {
                    Id = row.Id,
                    Name = row.Name
                };

                foreach (var column in columns)
                {
                    newRow[column.Id] = new CombinationCell
                    {
                        RowId = row.Id,
                        ColumnId = column.Id,
                    };
                }

                matrix[row.Id] = newRow;
            }

            return matrix;
        }

        private CombinationDTO MapToCombination(CombinationMatrix matrix, INormalRuleGroupProvider groupProvider)
        {
            return new CombinationDTO
            {
                IntersectionTitle = groupProvider.GroupName,
                Columns = groupProvider.Columns.Select(item => new ColumnDTO { Id = item.Id, Name = item.Name }).ToList(),
                Rows = matrix.ToRows()
            };
        }
    }
}
