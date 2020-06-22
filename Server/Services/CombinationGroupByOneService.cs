using FeatureDBPortal.Server.Data.Models;
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

            var selectedRowField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup);

            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

            //var groups = normalRules
            //    .GroupBy(normalRule => normalRule.GetPropertyValue<int?>(firstLayoutGroup + "Id"));

            // Maybe add firstLayoutGroup + "Id" == null
            var groups = normalRules
                .GroupBy(PropertyExpressionBuilder.Build<NormalRule, int?>(firstLayoutGroup + "Id").Compile());

            var nullGroup = groups.SingleOrDefault(group => group.Key == null);

            var orderedSelectedRowField = selectedRowField
                .ToList()
                .OrderBy(item => item.Name)
                .ToList();

            CombinationDictionary matrix = PrepareMatrix(orderedSelectedRowField);

            groups
                .ToList()
                .ForEach(group =>
                {
                    matrix[group.Key][group.Key] = new CombinationCell
                    {
                        RowId = group.Key,
                        ColumnId = group.Key,
                        Allow = nullGroup == null ? group.All(normalRule => normalRule.Allow != 0) : group.Union(nullGroup).All(normalRule => normalRule.Allow != 0)
                    };
                });

            var combination = new CombinationDTO
            {
                Headers = new List<ColumnTitleDTO>
                {
                    new ColumnTitleDTO { Id = -1, Name = firstLayoutGroup },
                    new ColumnTitleDTO { Id = -1, Name = "Allow" }
                },
                Rows = matrix.ToRows()
            };

            return await Task.FromResult(combination);
        }

        private static CombinationDictionary PrepareMatrix(List<IQueryableCombination> orderedSelectedRowField)
        {
            var matrix = new CombinationDictionary(orderedSelectedRowField.Count);

            orderedSelectedRowField
                .ForEach(rowItem =>
                {
                    var row = new RowDictionary(1) { Name = rowItem.Name };
                    matrix[rowItem.Id] = row;
                });
            return matrix;
        }
    }
}
