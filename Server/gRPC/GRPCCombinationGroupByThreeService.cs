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
//    public class GRPCCombinationGroupByThreeService : GRPCCombinationGroupService
//    {
//        public GRPCCombinationGroupByThreeService(FeaturesContext context)
//            : base(context)
//        {
//        }

//        async override public Task<CombinationGRPC> Combine(CombinationSearchGRPC search, IEnumerable<LayoutType> groupBy)
//        {
//            var firstLayoutGroup = groupBy.ElementAt(0);
//            var secondLayoutGroup = groupBy.ElementAt(1);
//            var thirdLayoutGroup = groupBy.ElementAt(2);

//            var selectedRowField = Context.GetPropertyValue<IQueryable<IQueryableEntity>>(firstLayoutGroup.ToString());
//            var selectedColumnField = Context.GetPropertyValue<IQueryable<IQueryableEntity>>(secondLayoutGroup.ToString());
//            var selectedCellField = Context.GetPropertyValue<IQueryable<IQueryableEntity>>(thirdLayoutGroup.ToString());

//            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

//            var firstGroupExpression = PropertyExpressionBuilder.Build<NormalRule, int?>(firstLayoutGroup + "Id").Compile();
//            var thirdGroupExpression = PropertyExpressionBuilder.Build<NormalRule, int?>(thirdLayoutGroup + "Id").Compile();

//            var groups = normalRules
//                .GroupBy(firstGroupExpression)
//                .Select(group => new
//                {
//                    RowId = group.Key,
//                    RowName = selectedRowField.SingleOrDefault(item => item.Id == group.Key)?.Name,
//                    Cells = group.Select(groupItem => new
//                    {
//                        RowId = group.Key,
//                        ColumnId = groupItem.GetPropertyValue<int?>(secondLayoutGroup + "Id"),
//                        Allow = group.All(item => item.Allow != 0),
//                        Items = group.GroupBy(thirdGroupExpression).Select(thirdGroup => new
//                        {
//                            ItemName = selectedCellField.SingleOrDefault(cellItem => cellItem.Id == thirdGroup.Key)?.Name,
//                            Allow = thirdGroup.All(i => i.Allow != 0)
//                        })
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
//                for (var y = 0; y < rowA.Cells.Count; y++)
//                {
//                    var columnA = rowA.Cells[y];

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
//                                Available = columnA.Allow,
//                                Items = columnA.Items?
//                                    .Where(cellItem => cellItem.Allow)
//                                .Select(cellItem => new CombinationItem
//                                {
//                                    RowId = rowKey,
//                                    ColumnId = columnKey,
//                                    Name = cellItem.ItemName
//                                })
//                            };
//                        }
//                        else
//                        {
//                            matrix[rowKey].Add(columnKey, new CombinationCell
//                            {
//                                RowId = rowKey,
//                                ColumnId = columnKey,
//                                Available = columnA.Allow,
//                                Items = columnA.Items?
//                                    .Where(cellItem => cellItem.Allow)
//                                .Select(cellItem => new CombinationItem
//                                {
//                                    RowId = rowKey,
//                                    ColumnId = columnKey,
//                                    Name = cellItem.ItemName
//                                })
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
//                            Available = columnA.Allow,
//                            Items = columnA.Items?
//                                .Where(cellItem => cellItem.Allow)
//                                .Select(cellItem => new CombinationItem
//                                {
//                                    RowId = rowKey,
//                                    ColumnId = columnKey,
//                                    Name = cellItem.ItemName
//                                })
//                        };
//                        matrix[rowKey] = newRow;
//                    }
//                }
//            }
            
//            var firstHeaderItem = new List<ColumnTitleGRPC> { new ColumnTitleGRPC
//            {
//                Name = $"{firstLayoutGroup} - {secondLayoutGroup} - {thirdLayoutGroup}"
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
