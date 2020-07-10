using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Shared;
using System;
using System.Linq;

namespace FeatureDBPortal.Client.Extensions
{
    public static class CombinationDTOExtensions
    {
        public static CombinationDTO ToDTO(this Combination model)
        {
            return new CombinationDTO
            {
                IntersectionTitle = model.IntersectionTitle,
                Rows = model.Rows.Select(row =>
                {
                    var newRow = new RowDTO
                    {
                        RowId = row.Value.Title.Id,
                        Title = new RowTitleDTO
                        {
                            Id = row.Value.Title.Id,
                            Name = row.Value.Title.Name
                        },
                        Cells = row.Value.Cells.Select(cell => new CellDTO
                        {
                            RowId = cell.Value.RowId,
                            ColumnId = cell.Value.ColumnId,
                            Available = cell.Value.Available,
                            Visible = cell.Value.Visible,
                            AllowMode = cell.Value.AllowMode.HasValue ? (AllowModeDTO)cell.Value.AllowMode : new Nullable<AllowModeDTO>(),
                            Name = cell.Value.Name,
                            //AggregateItems = cell.Value.Items == null ? string.Empty : string.Join(System.Environment.NewLine, cell.Value.Items.Select(item => item.Name)),
                            Items = cell.Value.Items?.Select(item => new CellItemDTO
                            {
                                RowId = item.RowId,
                                ColumnId = item.ColumnId,
                                ItemId = item.ItemId,
                                Name = item.Name
                            })
                        }).ToList()
                    };

                    return newRow;
                }).ToList(),
                Columns = model.Columns.Select(header => new ColumnDTO
                {
                    Id = header.Value.Id,
                    Name = header.Value.Name
                }).ToList()
            };
        }
    }
}
