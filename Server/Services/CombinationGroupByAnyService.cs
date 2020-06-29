﻿using FeatureDBPortal.Server.Data.Models;
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
            matrix[-1] = new RowDictionary(1);
            matrix[-1][-1] = new CombinationCell()
            {
                Available = available,
                Visible = visible,
                AllowMode = allowMode,
            };

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
