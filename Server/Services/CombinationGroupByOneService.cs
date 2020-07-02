using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByOneService : CombinationGroupService
    {
        public CombinationGroupByOneService(DbContext context)
            : base(context)
        {
        }

        async override public Task<CombinationDTO> Combine(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
        {
            var firstLayoutGroup = groupBy
                .ElementAt(0)
                .ToString();

            var orderedSelectedRowField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup)
                .Select(item => new QueryableCombination { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToList();

            IQueryable<NormalRule> normalRules = FilterNormalRules(search);

            // Maybe add firstLayoutGroup + "Id" == null
            var groups = normalRules
                .GroupBy(PropertyExpressionBuilder.Build<NormalRule, int?>(firstLayoutGroup + "Id").Compile());

            CombinationDictionary matrix = PrepareMatrix(orderedSelectedRowField);

            groups
                .ToList()
                .ForEach(group =>
                {
                    var key = group.Key;
                    matrix[key][key] = new CombinationCell
                    {
                        RowId = group.Key,
                        ColumnId = -1,
                        Available = group.All(normalRule => (AllowMode)normalRule.Allow != AllowMode.No),
                        Visible = group.All(normalRule => (AllowMode)normalRule.Allow == AllowMode.A),
                        Name = "Allow"
                    };
                    matrix[key][key].AllowMode = Allower.GetMode(matrix[key][key].Visible.Value, matrix[key][key].Available.Value);
                });

            var combination = new CombinationDTO
            {
                IntersectionTitle = firstLayoutGroup,
                Columns = new List<ColumnDTO>
                {
                    new ColumnDTO { Id = -1, Name = "Allow" }
                },
                Rows = matrix.ToRows()
            };

            return await Task.FromResult(combination);
        }

        private static CombinationDictionary PrepareMatrix(List<QueryableCombination> orderedSelectedRowField)
        {
            var matrix = new CombinationDictionary(orderedSelectedRowField.Count);

            orderedSelectedRowField
                .ForEach(rowItem =>
                {
                    var row = new RowDictionary(1) { RowId = rowItem.Id, Name = rowItem.Name };
                    matrix[rowItem.Id] = row;
                });

            return matrix;
        }
    }
}
