﻿using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using GrpcCombination;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Extensions
{
    public static class CombinationDictionaryExtensions
    {
        public static IEnumerable<RowDTO> ToRows(this CombinationDictionary matrix)
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
                })
            });
        }

        public static IEnumerable<RowGRPC> ToGRPCRows(this CombinationDictionary matrix)
        {
            return matrix.Values.Select(row =>
            {
                var newRow = new RowGRPC();
                newRow.TitleCell = new CombinationCellGRPC { Name = row.Name };
                newRow.Cells.AddRange(row.Values.Select(cell =>
                {
                    var newCell = new CombinationCellGRPC();
                    newCell.Allow = cell.Allow;

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