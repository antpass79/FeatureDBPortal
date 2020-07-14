using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using GrpcCombination;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Extensions
{
    public static class CombinationDictionaryExtensions
    {
        public static IReadOnlyList<RowDTO> ToRows(this CombinationMatrix matrix)
        {
            return matrix.Values
                .Where(item => item.Id.HasValue)
                .Select(row => new RowDTO
            {
                RowId = row.Id,
                Title = new RowTitleDTO
                {
                    Id = row.Id,
                    Name = row.Name
                },
                Cells = row.Values.Select(cell => new CellDTO
                {
                    RowId = cell.RowId,
                    ColumnId = cell.ColumnId,

                    Available = cell.Available,
                    Visible = cell.Visible,
                    AllowMode = cell.AllowMode.HasValue ? (AllowModeDTO)cell.AllowMode : new Nullable<AllowModeDTO>(),

                    Items = cell.Items?.Select(itemCell => new CellItemDTO
                    {
                        RowId = itemCell.RowId,
                        ColumnId = itemCell.ColumnId,
                        ItemId = itemCell.ItemId,
                        Name = itemCell.Name
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        public static IEnumerable<RowGRPC> ToGRPCRows(this CombinationMatrix matrix)
        {
            return matrix.Values.Select(row =>
            {
                var newRow = new RowGRPC();
                newRow.TitleCell = new CombinationCellGRPC { Name = row.Name };
                newRow.Cells.AddRange(row.Values.Select(cell =>
                {
                    var newCell = new CombinationCellGRPC();
                    newCell.Allow = cell.Available;

                    if (cell.Items != null && cell.Items.Count() > 0)
                    {
                        newCell.Items.AddRange(cell.Items.Select(itemCell => new CombinationItemGRPC
                        {
                            RowId = itemCell.RowId,
                            ColumnId = itemCell.ColumnId,
                            ItemId = itemCell.ItemId,
                            Name = itemCell.Name
                        }));
                    }

                    return newCell;
                }));

                return newRow;
            });
        }
    }
}
