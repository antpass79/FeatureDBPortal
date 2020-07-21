using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Providers;
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
        public CombinationGroupByAnyService(FeaturesContext context, IVersionProvider versionProvider, GroupProviderBuilder groupProviderBuilder)
            : base(context, versionProvider, groupProviderBuilder)
        {
        }

        async override protected Task<CombinationDTO> BuildCombination(CombinationSearchDTO search, IQueryable<NormalRule> normalRules, GroupProviderBuilder groupProviderBuilder)
        {
            bool available = false;
            bool visible = false;
            AllowMode allowMode = AllowMode.No;

            if (search.ProbeId.HasValue)
            {
                // Probe 50: TLC 3-13 => 2 transducers
                var transducers = Context.ProbeTransducers
                    .Where(item => item.ProbeId == search.ProbeId)
                    .ToList();

                foreach (var transducer in transducers)
                {
                    var transducerNormalRules = normalRules.
                        Where(normalRule => !normalRule.TransducerType.HasValue || transducer.TransducerType == normalRule.TransducerType);

                    available |= transducerNormalRules.All(normalRule => (AllowMode)normalRule.Allow != AllowMode.No);
                    visible |= transducerNormalRules.All(normalRule => (AllowMode)normalRule.Allow == AllowMode.A);
                }
            }
            else
            {
                available = normalRules.All(normalRule => (AllowMode)normalRule.Allow != AllowMode.No);                
                visible = normalRules.All(normalRule => (AllowMode)normalRule.Allow == AllowMode.A);
            }

            allowMode = Allower.GetMode(visible, available);

            var matrix = new CombinationMatrix(1);
            matrix[-1] = new CombinationRow(1) { Id = -1 };
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
                        Id = -1,
                        Name = "Value"
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
