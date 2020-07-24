using FeatureDBPortal.Server.Builders;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByOneProvider : IGroupProvider
    {
        private readonly GroupProperties _groupProperties;
        private readonly IAllowModeProvider _allowModeProvider;
        private readonly CombinationIndexerBuilder _combinationIndexerBuilder;

        public GroupByOneProvider(
            GroupProperties groupProperties,
            IAllowModeProvider allowModeProvider,
            CombinationIndexerBuilder combinationIndexerBuilder)
        {
            _groupProperties = groupProperties;
            _rows = _groupProperties.GroupableItems.Values.ToList();
            _columns = new List<QueryableEntity>
                            {
                                new QueryableEntity
                                {
                                    Id = -1,
                                    Name = "Allow"
                                }
                            };

            _allowModeProvider = allowModeProvider;
            _combinationIndexerBuilder = combinationIndexerBuilder;
        }

        public string GroupName => _groupProperties.NormalRulePropertyName;

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

            // Maybe add firstLayoutGroup + "Id" == null
            var groups = parameters.NormalRules
                .GroupBy(_groupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_groupProperties.DiscardItemIds.Contains(item.Key.Value));

            Parallel.ForEach(groups, (group) =>
            {
                var cell = combinationIndexer[group.Key, -1];
                var allowModeProperties = _allowModeProvider.Properties(group, parameters.ProbeId);

                cell.Visible = allowModeProperties.Visible;
                cell.Available = allowModeProperties.Available;
                cell.AllowMode = allowModeProperties.AllowMode;
            });

            return combinationIndexer.Combination;
        }
    }
}
