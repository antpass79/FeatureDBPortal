using FeatureDBPortal.Server.Builders;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByTwoProvider : IGroupProvider
    {
        private readonly GroupProperties _rowGroupProperties;
        private readonly GroupProperties _columnGroupProperties;
        private readonly IAllowModeProvider _allowModeProvider;
        private readonly CombinationIndexerBuilder _combinationIndexerBuilder;

        public GroupByTwoProvider(
            GroupProperties rowGroupProperties,
            GroupProperties columnGroupProperties,
            IAllowModeProvider allowModeProvider,
            CombinationIndexerBuilder combinationIndexerBuilder)
        {
            _rowGroupProperties = rowGroupProperties;
            _columnGroupProperties = columnGroupProperties;

            _rows = _rowGroupProperties.GroupableItems.Values.ToList();
            _columns = _columnGroupProperties.GroupableItems.Values.ToList();

            _allowModeProvider = allowModeProvider;
            _combinationIndexerBuilder = combinationIndexerBuilder;
        }

        public string GroupName => $"{_rowGroupProperties.LayoutType} / {_columnGroupProperties.LayoutType}";

        IReadOnlyList<QueryableEntity> _rows;
        public IReadOnlyList<QueryableEntity> Rows => _rows;
        IReadOnlyList<QueryableEntity> _columns;
        public IReadOnlyList<QueryableEntity> Columns => _columns;

        public CombinationDTO Group(IList<NormalRule> normalRules, GroupParameters parameters)
        {
            var combinationIndexer = _combinationIndexerBuilder
                .Rows(Rows)
                .Columns(Columns)
                .Title(GroupName)
                .Build();

            var groups = normalRules
                .GroupBy(_rowGroupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_rowGroupProperties.DiscardItemIds.Contains(item.Key.Value));

            using (var watcher = new Watcher("GROUP BY 2 PARALLEL"))
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
                        }
                    });
                });
            }

            return combinationIndexer.Combination;
        }
    }
}
