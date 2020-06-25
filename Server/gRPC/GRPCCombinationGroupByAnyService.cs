﻿using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using GrpcCombination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.gRPC
{
    public class GRPCCombinationGroupByAnyService : GRPCCombinationGroupService
    {
        public GRPCCombinationGroupByAnyService(DbContext context)
            : base(context)
        {
        }

        async override public Task<CombinationGRPC> Combine(CombinationSearchGRPC search, IEnumerable<LayoutType> groupBy)
        {
            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);
            if (normalRules.Count() == 0)
                throw new ArgumentOutOfRangeException();

            var allow = normalRules.All(normalRule => normalRule.Allow != 0);

            var matrix = new CombinationDictionary(1);
            matrix[-1] = new RowDictionary(1);
            matrix[-1][-1] = new CombinationCell() { Allow = allow };

            var combination = new CombinationGRPC();
            combination.Headers.Add(new ColumnTitleGRPC { Id = -1, Name = "Allow" });
            combination.Rows.AddRange(matrix.Values.Select(item =>
            {
                var newRow = new RowGRPC();
                newRow.TitleCell = new CombinationCellGRPC();
                newRow.Cells.AddRange(item.Values.Select(innerItem => new CombinationCellGRPC
                {
                    Allow = innerItem.Allow
                }));

                return newRow;
            }));

            return await Task.FromResult(combination);
        }
    }
}