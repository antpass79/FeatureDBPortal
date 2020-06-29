using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Shared;
using GrpcCombination;
using System;
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
                    RowId = row.RowId,
                    Cells = row.Cells.Select(cell => new CombinationCell
                    {
                        RowId = cell.RowId,
                        ColumnId = cell.ColumnId,
                        Available = cell.Available,
                        Visible = cell.Visible,
                        AllowMode = cell.AllowMode.HasValue ? (AllowMode)cell.AllowMode : new Nullable<AllowMode>(),
                        Name = cell.Name,
                        AggregateItems = cell.Items == null ? string.Empty : string.Join(System.Environment.NewLine, cell.Items.Select(item => item.Name)),
                        //Items = cell.Items?.Select(item => new CombinationItem
                        //{
                        //    RowId = item.RowId,
                        //    ColumnId = item.ColumnId,
                        //    ItemId = item.ItemId,
                        //    Name = item.Name
                        //})
                    }).ToList(),
                    TitleCell = new CombinationCell()
                    {
                        RowId = row.TitleCell.RowId,
                        ColumnId = row.TitleCell.ColumnId,
                        Available = row.TitleCell.Available,
                        Name = row.TitleCell.Name
                    }
                }).ToList(),
                Headers = dto.Headers.Select(header => new ColumnTitle
                {
                    Id = header.Id,
                    Name = header.Name
                }).ToList()
            };
        }

        public static Combination ToModel(this CombinationGRPC dto)
        {
            return new Combination
            {
                Rows = dto.Rows.Select(row => new Row
                {
                    Cells = row.Cells.Select(cell => new CombinationCell
                    {
                        RowId = cell.RowId,
                        ColumnId = cell.ColumnId,
                        Available = cell.Allow,
                        Name = cell.Name,
                        AggregateItems = cell.Items == null ? string.Empty : string.Join(System.Environment.NewLine, cell.Items.Select(item => item.Name)),
                        //Items = cell.Items?.Select(item => new CombinationItem
                        //{
                        //    RowId = item.RowId,
                        //    ColumnId = item.ColumnId,
                        //    ItemId = item.ItemId,
                        //    Name = item.Name
                        //})
                    }).ToList(),
                    TitleCell = new CombinationCell()
                    {
                        RowId = row.TitleCell.RowId,
                        ColumnId = row.TitleCell.ColumnId,
                        Available = row.TitleCell.Allow,
                        Name = row.TitleCell.Name
                    }
                }).ToList(),
                Headers = dto.Headers.Select(header => new ColumnTitle
                {
                    Id = header.Id,
                    Name = header.Name
                }).ToList()
            };
        }
    }
}
