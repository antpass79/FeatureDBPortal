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
                IntersectionTitle = dto.IntersectionTitle,
                Rows = dto.Rows.Select(row =>
                {
                    var newRow = new Row
                    {
                        Id = row.Title.Id,
                        Title = new RowTitle
                        {
                            Id = row.Title.Id,
                            Name = row.Title.Name
                        },
                        Cells = row.Cells.Select(cell => new Cell
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
                        }).ToList()
                    };

                    return newRow;
                }).ToDictionary(item => item.Id.Value),
                Columns = dto.Columns.Select(header => new Column
                {
                    Id = header.Id,
                    Name = header.Name
                }).ToDictionary(item => item.Id.Value)
            };
        }

        public static Combination ToModel(this CombinationGRPC dto)
        {
            return new Combination
            {
                Rows = dto.Rows.Select(row => new Row
                {
                    Id = row.TitleCell.RowId,
                    Title = new RowTitle
                    {
                        Id = row.TitleCell.RowId,
                        Name = row.TitleCell.Name
                    },
                    Cells = row.Cells.Select(cell => new Cell
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
                    }).ToList()
                }).ToDictionary(item => item.Id.Value),
                Columns = dto.Headers.Select(header => new Column
                {
                    Id = header.Id,
                    Name = header.Name
                }).ToDictionary(item => item.Id.Value)
            };
        }
    }
}
