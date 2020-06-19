using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Shared;
using System.Linq;

namespace FeatureDBPortal.Client.Extensions
{
    public static class CombinationExtensions
    {
        public static Combination ToModel(this CombinationDTO dto)
        {
            return new Combination
            {
                Rows = dto.Rows.Select(row => new Row
                {
                    Cells = row.Cells.Select(cell => new CombinationCell
                    {
                        RowId = cell.RowId,
                        ColumnId = cell.ColumnId,
                        Allow = cell.Allow,
                        Name = cell.Name,
                        Items = cell.Items?.Select(item => new CombinationItem
                        {
                            RowId = item.RowId,
                            ColumnId = item.ColumnId,
                            ItemId = item.ItemId,
                            Name = item.Name,
                        })
                    }).ToList(),
                    TitleCell = new CombinationCell()
                    {
                        RowId = row.TitleCell.RowId,
                        ColumnId = row.TitleCell.ColumnId,
                        Allow = row.TitleCell.Allow,
                        Name = row.TitleCell.Name
                    }
                }).ToList(),
                Headers = dto.Headers.Select(header => new ColumnTitle
                {
                    Id = header.Id,
                    Name = header.Name
                })
            };
        }
    }
}
