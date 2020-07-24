using FeatureDBPortal.Server.Builders;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByAnyProvider : IGroupProvider
    {
        private readonly IAllowModeProvider _allowModeProvider;
        private readonly CombinationIndexerBuilder _combinationIndexerBuilder;

        public GroupByAnyProvider(
            IAllowModeProvider allowModeProvider,
            CombinationIndexerBuilder combinationIndexerBuilder)
        {
            _rows = new List<QueryableEntity>
                            {
                                new QueryableEntity
                                {
                                    Id = -1,
                                    Name = "Value"
                                }
                            };
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

        public string GroupName => string.Empty;

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

            var cell = combinationIndexer[-1, -1];
            var allowModeProperties = _allowModeProvider.Properties(parameters.NormalRules, parameters.ProbeId);

            cell.Visible = allowModeProperties.Visible;
            cell.Available = allowModeProperties.Available;
            cell.AllowMode = allowModeProperties.AllowMode;

            return combinationIndexer.Combination;
        }
    }
}
