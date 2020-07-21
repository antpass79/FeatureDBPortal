using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByOneProvider : IGroupProvider
    {
        private readonly GroupProperties _groupProperties;
        public GroupByOneProvider(GroupProperties groupProperties)
        {
            _groupProperties = groupProperties;
            _rows = _groupProperties.GroupableItems.Values.ToList();
        }

        public string GroupName => _groupProperties.NormalRulePropertyName;


        IReadOnlyList<QueryableCombination> _rows;
        public IReadOnlyList<QueryableCombination> Rows => _rows;
        public IReadOnlyList<QueryableCombination> Columns => throw new NotSupportedException();

        public CombinationDTO Group(IList<NormalRule> normalRules)
        {
            // Maybe add firstLayoutGroup + "Id" == null
            var groups = normalRules
                .GroupBy(_groupProperties.GroupExpression)
                .Where(item => item.Key.HasValue && !_groupProperties.DiscardItemIds.Contains(item.Key.Value))
                .Select(group => new RowDTO
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
                            Available = group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No),
                            Visible = group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A),
                            AllowMode = (AllowModeDTO)Allower.GetMode(group.All(item => (AllowModeDTO)item.Allow == AllowModeDTO.A), group.All(item => (AllowModeDTO)item.Allow != AllowModeDTO.No))
                        }
                    }
                    }).ToList()
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
