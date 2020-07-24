using FeatureDBPortal.Server.Builders;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByThreeProvider : IGroupProvider
    {
        private readonly GroupProperties _rowGroupProperties;
        private readonly GroupProperties _columnGroupProperties;
        private readonly GroupProperties _cellGroupProperties;
        private readonly IAllowModeProvider _allowModeProvider;
        private readonly CombinationIndexerBuilder _combinationIndexerBuilder;

        public GroupByThreeProvider(
            GroupProperties rowGroupProperties,
            GroupProperties columnGroupProperties,
            GroupProperties cellGroupProperties,
            IAllowModeProvider allowModeProvider,
            CombinationIndexerBuilder combinationIndexerBuilder)
        {
            _rowGroupProperties = rowGroupProperties;
            _columnGroupProperties = columnGroupProperties;
            _cellGroupProperties = cellGroupProperties;

            _rows = _rowGroupProperties.GroupableItems.Values.ToList();
            _columns = _columnGroupProperties.GroupableItems.Values.ToList();

            _allowModeProvider = allowModeProvider;
            _combinationIndexerBuilder = combinationIndexerBuilder;
        }

        public string GroupName => $"{_rowGroupProperties.LayoutType} / {_columnGroupProperties.LayoutType} / {_cellGroupProperties.LayoutType}";

        IReadOnlyList<QueryableEntity> _rows;
        public IReadOnlyList<QueryableEntity> Rows => _rows;
        IReadOnlyList<QueryableEntity> _columns;
        public IReadOnlyList<QueryableEntity> Columns => _columns;


        public CombinationDTO Group(GroupParameters parameters)
        {
            var combinationIndexer = _combinationIndexerBuilder
                .Rows(Rows)
                .Columns(Columns)
                .Title(GroupName)
                .Build();

            var groups = parameters.NormalRules
                .GroupBy(_rowGroupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_rowGroupProperties.DiscardItemIds.Contains(item.Key.Value));

            using (var watcher = new Watcher("GROUP BY 3 PARALLEL"))
            {
                Parallel.ForEach(groups, (group) =>
                {
                    Parallel.ForEach(group, (normalRule) =>
                    {
                        var columnId = normalRule.GetPropertyIdByGroupNameId(_columnGroupProperties.NormalRulePropertyNameId);
                        if (columnId.HasValue)
                        {
                            var cell = combinationIndexer[group.Key, columnId];
                            var allowModeProperties = _allowModeProvider.Properties(group, parameters.ProbeId);

                            cell.Visible = allowModeProperties.Visible;
                            cell.Available = allowModeProperties.Available;
                            cell.AllowMode = allowModeProperties.AllowMode;

                            var innerGroup = group
                                .GroupBy(_cellGroupProperties.GroupExpression)
                                .Where(item => item.All(i => i.Allow != 0) && item.Key.HasValue && _cellGroupProperties.GroupableItems.ContainsKey(item.Key.Value));

                            cell.Items = innerGroup.Select(thirdGroup => new CellItemDTO
                            {
                                Name = _cellGroupProperties.GroupableItems[thirdGroup.Key.Value].Name
                            })
                                .ToList();
                        }
                    });
                });
            }

            return combinationIndexer.Combination;
        }
    }
}
