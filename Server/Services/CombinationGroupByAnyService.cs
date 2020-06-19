using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System;
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
            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);
            if (normalRules.Count() == 0)
                throw new ArgumentOutOfRangeException();

            var allow = normalRules.All(normalRule => normalRule.Allow != 0);

            var matrix = new CombinationDictionary();
            matrix.Add(-1, new RowDictionary());
            matrix[-1].Add(-1, new CombinationCell() { Allow = allow });

            var combination = new CombinationDTO
            {
                Headers = new List<ColumnTitleDTO>
                {
                    new ColumnTitleDTO { Id = -1, Name = "Allow" },
                },
                Rows = matrix.Values.Select(item => new RowDTO
                {
                    TitleCell = new CombinationCellDTO(),
                    Cells = item.Values.Select(innerItem => new CombinationCellDTO
                    {
                        Allow = innerItem.Allow
                    }).ToList()
                }).ToList()
            };

            return await Task.FromResult(combination);
        }
    }
}
