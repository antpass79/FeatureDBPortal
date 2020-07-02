using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class CombinationGroupByAnyService : CombinationGroupService
    {
        public CombinationGroupByAnyService(DbContext context)
            : base(context)
        {
        }

        async override public Task<CombinationDTO> Combine(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
        {
            IQueryable<NormalRule> normalRules = FilterNormalRules(search);
            if (normalRules.Count() == 0)
                return await Task.FromResult(new CombinationDTO());

            var available = normalRules.All(normalRule => (AllowMode)normalRule.Allow != AllowMode.No);
            var visible = normalRules.All(normalRule => (AllowMode)normalRule.Allow == AllowMode.A);
            var allowMode = Allower.GetMode(visible, available);

            var matrix = new CombinationDictionary(1);
            matrix[-1] = new RowDictionary(1) { RowId = -1 };
            matrix[-1][-1] = new CombinationCell()
            {
                RowId = -1,
                ColumnId = -1,
                Available = available,
                Visible = visible,
                AllowMode = allowMode,
            };

            var combination = new CombinationDTO
            {
                IntersectionTitle = string.Empty,
                Columns = new List<ColumnDTO>
                {
                    new ColumnDTO { Id = -1, Name = "Allow" },
                },
                Rows = matrix.Values.Select(item => new RowDTO
                {
                    RowId = -1,
                    Title = new RowTitleDTO
                    {
                        Id = -1
                    },
                    Cells = item.Values.Select(innerItem => new CellDTO
                    {
                        RowId = -1,
                        ColumnId = -1,
                        Available = innerItem.Available,
                        Visible = innerItem.Visible,
                        AllowMode = (AllowModeDTO)innerItem.AllowMode,
                    }).ToList()
                }).ToList()
            };

            return await Task.FromResult(combination);
        }
    }
}
