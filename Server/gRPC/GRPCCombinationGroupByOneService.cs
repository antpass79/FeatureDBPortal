﻿//using FeatureDBPortal.Server.Data.Models.RD;
//using FeatureDBPortal.Server.Extensions;
//using FeatureDBPortal.Server.Models;
//using FeatureDBPortal.Server.Utils;
//using GrpcCombination;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FeatureDBPortal.Server.gRPC
//{
//    public class GRPCCombinationGroupByOneService : GRPCCombinationGroupService
//    {
//        public GRPCCombinationGroupByOneService(FeaturesContext context)
//            : base(context)
//        {
//        }

//        async override public Task<CombinationGRPC> Combine(CombinationSearchGRPC search, IEnumerable<LayoutType> groupBy)
//        {
//            var firstLayoutGroup = groupBy
//                .ElementAt(0)
//                .ToString();

//            var selectedRowField = Context.GetPropertyValue<IQueryable<IQueryableEntity>>(firstLayoutGroup);

//            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

//            //var groups = normalRules
//            //    .GroupBy(normalRule => normalRule.GetPropertyValue<int?>(firstLayoutGroup + "Id"));

//            // Maybe add firstLayoutGroup + "Id" == null
//            var groups = normalRules
//                .GroupBy(PropertyExpressionBuilder.Build<NormalRule, int?>(firstLayoutGroup + "Id").Compile());

//            var nullGroup = groups.SingleOrDefault(group => group.Key == null);

//            var orderedSelectedRowField = selectedRowField
//                .ToList()
//                .OrderBy(item => item.Name)
//                .ToList();

//            CombinationMatrix matrix = PrepareMatrix(orderedSelectedRowField);

//            groups
//                .ToList()
//                .ForEach(group =>
//                {
//                    var key = group.Key.HasValue ? group.Key.Value : -1;
//                    matrix[key][key] = new CombinationCell
//                    {
//                        RowId = group.Key,
//                        ColumnId = group.Key,
//                        Available = nullGroup == null ? group.All(normalRule => normalRule.Allow != 0) : group.Union(nullGroup).All(normalRule => normalRule.Allow != 0)
//                    };
//                });

//            var combination = new CombinationGRPC();
//            combination.Headers.AddRange(new List<ColumnTitleGRPC>
//                {
//                    new ColumnTitleGRPC { Id = -1, Name = firstLayoutGroup },
//                    new ColumnTitleGRPC { Id = -1, Name = "Allow" }
//                });
//            combination.Rows.AddRange(matrix.ToGRPCRows());

//            return await Task.FromResult(combination);
//        }

//        private static CombinationMatrix PrepareMatrix(List<IQueryableEntity> orderedSelectedRowField)
//        {
//            var matrix = new CombinationMatrix(orderedSelectedRowField.Count);

//            orderedSelectedRowField
//                .ForEach(rowItem =>
//                {
//                    var row = new CombinationRow(1) { Id = rowItem.Id, Name = rowItem.Name };
//                    matrix[rowItem.Id] = row;
//                });

//            return matrix;
//        }
//    }
//}
