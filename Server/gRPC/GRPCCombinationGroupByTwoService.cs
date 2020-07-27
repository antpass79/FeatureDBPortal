//using FeatureDBPortal.Server.Data.Models.RD;
//using FeatureDBPortal.Server.Extensions;
//using FeatureDBPortal.Server.Models;
//using FeatureDBPortal.Server.Utils;
//using GrpcCombination;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FeatureDBPortal.Server.gRPC
//{
//    public class GRPCCombinationGroupByTwoService : GRPCCombinationGroupService
//    {
//        public GRPCCombinationGroupByTwoService(FeaturesContext context)
//            : base(context)
//        {
//        }

//        async override public Task<CombinationGRPC> Combine(CombinationSearchGRPC search, IEnumerable<LayoutType> groupBy)
//        {
//            var firstLayoutGroup = groupBy
//                .ElementAt(0)
//                .ToString();
//            var secondLayoutGroup = groupBy
//                .ElementAt(1)
//                .ToString();

//            var selectedRowField = Context.GetPropertyValue<IQueryable<IQueryableEntity>>(firstLayoutGroup);
//            var selectedColumnField = Context.GetPropertyValue<IQueryable<IQueryableEntity>>(secondLayoutGroup);

//            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

//            var groups = normalRules
//                .GroupBy(PropertyExpressionBuilder.Build<NormalRule, int?>(firstLayoutGroup + "Id").Compile())
//                .Select(group => new
//                {
//                    RowId = group.Key,
//                    RowName = selectedRowField.SingleOrDefault(item => item.Id == group.Key)?.Name,
//                    Combinations = group.Select(groupItem => new
//                    {
//                        RowId = group.Key,
//                        Availability = group.All(item => item.Allow != 0),
//                        ColumnId = groupItem.GetPropertyValue<int?>(secondLayoutGroup + "Id")
//                    }).ToList()
//                }).ToList();

//            var orderedSelectedRowField = selectedRowField
//                .ToList()
//                .OrderBy(item => item.Name)
//                .ToList();
//            var orderedSelectedColumnField = selectedColumnField
//                .ToList()
//                .OrderBy(item => item.Name)
//                .ToList();

//            CombinationMatrix matrix = PrepareMatrix(orderedSelectedRowField, orderedSelectedColumnField);

//            for (var x = 0; x < groups.Count; x++)
//            {
//                var rowA = groups[x];
//                for (var y = 0; y < rowA.Combinations.Count; y++)
//                {
//                    var columnA = rowA.Combinations[y];

//                    var rowKey = columnA.RowId.HasValue ? columnA.RowId : -1;
//                    var columnKey = columnA.ColumnId.HasValue ? columnA.ColumnId : -1;

//                    if (matrix.ContainsKey(rowKey))
//                    {
//                        var selectedRow = matrix[rowKey];

//                        if (selectedRow.ContainsKey(columnKey))
//                        {
//                            matrix[rowKey][columnKey] = new CombinationCell
//                            {
//                                RowId = rowKey,
//                                ColumnId = columnKey,
//                                Available = columnA.Availability
//                            };
//                        }
//                        else
//                        {
//                            matrix[rowKey].Add(columnKey, new CombinationCell
//                            {
//                                RowId = rowKey,
//                                ColumnId = columnKey,
//                                Available = columnA.Availability
//                            });
//                        }
//                    }
//                    else
//                    {
//                        var newRow = new CombinationRow(1);
//                        newRow[columnKey] = new CombinationCell()
//                        {
//                            RowId = rowKey,
//                            ColumnId = columnKey,
//                            Available = columnA.Availability
//                        };
//                        matrix[rowKey] = newRow;
//                    }
//                }
//            }

//            var firstHeaderItem = new List<ColumnTitleGRPC> { new ColumnTitleGRPC
//            {
//                Name = $"{firstLayoutGroup} / {secondLayoutGroup}"
//            } };

//            var combination = new CombinationGRPC();
//            combination.Headers.AddRange(firstHeaderItem.Union(orderedSelectedColumnField.Select(item => new ColumnTitleGRPC { Id = item.Id, Name = item.Name })));
//            combination.Rows.AddRange(matrix.ToGRPCRows());

//            return await Task.FromResult(combination);
//        }

//        private static CombinationMatrix PrepareMatrix(List<IQueryableEntity> orderedSelectedRowField, List<IQueryableEntity> orderedSelectedColumnField)
//        {
//            var matrix = new CombinationMatrix(orderedSelectedRowField.Count);

//            orderedSelectedRowField
//                .ForEach(rowItem =>
//                {
//                    var row = new CombinationRow(orderedSelectedColumnField.Count);
//                    row.Id = rowItem.Id;
//                    row.Name = rowItem.Name;
//                    orderedSelectedColumnField
//                    .ForEach(columnItem =>
//                    {
//                        row[columnItem.Id] = new CombinationCell
//                        {
//                            RowId = rowItem.Id,
//                            ColumnId = columnItem.Id
//                        };
//                    });
//                    matrix[rowItem.Id] = row;
//                });
//            return matrix;
//        }
//    }
//}
