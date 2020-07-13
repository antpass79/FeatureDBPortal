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
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByOneService : CombinationGroupService
    {
        public CombinationGroupByOneService(DbContext context, IVersionProvider versionProvider)
            : base(context, versionProvider)
        {
        }

        async override protected Task<CombinationDTO> GroupNormalRules(CombinationSearchDTO search, IQueryable<NormalRule> normalRules)
        {
            var firstLayoutGroup = search.RowLayout.ToNormalRulePropertyName();

            var fakeIds = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(search.RowLayout.ToTableName())
                .ToList()
                .Where(item => item.IsFake)
                .Select(item => item.Id)
                .ToList();

            var orderedSelectedRowField = Context.GetPropertyValue<IQueryable<IQueryableCombination>>(search.RowLayout.ToTableName())
                .AsEnumerable()
                .Where(item => !item.IsFake)
                .Select(item => new QueryableCombination { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToList();

            // If in output there are Model or Country:
            // - Foreach group take id (in case of model is the ModelId)
            // - Build the default version with group.Id and search.CountryId
            // - Filter rules with the default version (see FilterNormalRules function in the base class)
            // NOTE: only if there isn't a version in the search

            // Maybe add firstLayoutGroup + "Id" == null
            var groups = normalRules
                .GroupBy(PropertyExpressionBuilder.Build<NormalRule, int?>(search.RowLayout.ToNormalRulePropertyNameId()).Compile());

            CombinationDictionary matrix = PrepareMatrix(orderedSelectedRowField);

            groups
                .ToList()
                .Where(item => item.Key.HasValue && !fakeIds.Contains(item.Key.Value))
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
