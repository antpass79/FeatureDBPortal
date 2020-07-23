using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupByAnyProvider : IGroupProvider
    {
        private readonly IAllowModeProvider _allowModeProvider;

        public GroupByAnyProvider(IAllowModeProvider allowModeProvider)
        {
            _allowModeProvider = allowModeProvider;
        }

        public string GroupName => string.Empty;

        public IReadOnlyList<QueryableEntity> Rows => throw new NotImplementedException();
        public IReadOnlyList<QueryableEntity> Columns => throw new NotSupportedException();

        public CombinationDTO Group(GroupParameters parameters)
        {
            var allowModeProperties = _allowModeProvider.Properties(parameters.NormalRules, parameters.ProbeId);

            var combination = new CombinationDTO
            {
                IntersectionTitle = GroupName,
                Columns = new List<ColumnDTO>
                {
                    new ColumnDTO { Id = -1, Name = "Allow" },
                },
                Rows = new List<RowDTO>
                {
                    new RowDTO
                    {
                        RowId = -1,
                        Title = new RowTitleDTO
                        {
                            Id = -1,
                            Name = "Value"
                        },
                        Cells = new List<CellDTO>
                        {
                            new CellDTO
                            {
                                RowId = -1,
                                ColumnId = -1,
                                Available = allowModeProperties.Available,
                                Visible = allowModeProperties.Visible,
                                AllowMode = allowModeProperties.AllowMode
                            }
                        }
                    }
                }
            };

            return combination;
        }
    }
}
