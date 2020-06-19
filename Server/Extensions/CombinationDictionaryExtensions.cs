using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Extensions
{
    public static class CombinationDictionaryExtensions
    {
        public static IList<RowDTO> ToRows(this CombinationDictionary matrix)
        {
            return matrix.Values.Select(row => new RowDTO
            {
                TitleCell = new CombinationCellDTO { Name = row.Name },
                Cells = row.Values.Select(cell => new CombinationCellDTO
                {
                    Allow = cell.Allow,
                    Items = cell.Items?.Select(itemCell => new CombinationItemDTO
                    {
                        RowId = itemCell.RowId,
                        ColumnId = itemCell.ColumnId,
                        ItemId = itemCell.ItemId,
                        Name = itemCell.Name
                    })
                }).ToList()
            }).ToList();
        }
    }
}
