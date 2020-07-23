using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByOneProvider : IGroupProvider
    {
        private readonly GroupProperties _groupProperties;
        private readonly IAllowModeProvider _allowModeProvider;

        public GroupByOneProvider(
            GroupProperties groupProperties,
            IAllowModeProvider allowModeProvider)
        {
            _groupProperties = groupProperties;
            _rows = _groupProperties.GroupableItems.Values.ToList();

            _allowModeProvider = allowModeProvider;
        }

        public string GroupName => _groupProperties.NormalRulePropertyName;

        IReadOnlyList<QueryableEntity> _rows;
        public IReadOnlyList<QueryableEntity> Rows => _rows;
        public IReadOnlyList<QueryableEntity> Columns => throw new NotSupportedException();

        public CombinationDTO Group(GroupParameters parameters)
        {
            // Maybe add firstLayoutGroup + "Id" == null
            var groups = parameters.NormalRules
                .GroupBy(_groupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_groupProperties.DiscardItemIds.Contains(item.Key.Value))
                .Select(group =>
                    {
                        var allowModeProperties = _allowModeProvider.Properties(group, parameters.ProbeId);

                        var newRow = new RowDTO
                        {
                            RowId = group.Key,
                            Title = new RowTitleDTO
                            {
                                Id = group.Key,
                                Name = _groupProperties.GroupableItems[group.Key.Value].Name
                            },
                            Cells = new List<CellDTO>
                            {
                                new CellDTO
                                {
                                    RowId = group.Key,
                                    ColumnId = -1,
                                    Visible = allowModeProperties.Visible,
                                    Available = allowModeProperties.Available,
                                    AllowMode = allowModeProperties.AllowMode
                                }
                            }
                        };

                        return newRow;
                    }
                ).ToList()
                .OrderBy(item => item.Title.Name)
                .ToList();

            var combination = new CombinationDTO
            {
                IntersectionTitle = GroupName,
                Columns = new List<ColumnDTO>
                {
                    new ColumnDTO { Id = -1, Name = "Allow" }
                },
                Rows = groups
            };

            return combination;
        }
    }
}
