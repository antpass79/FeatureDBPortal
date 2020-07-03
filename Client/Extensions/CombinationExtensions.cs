using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Shared;
using GrpcCombination;
using System;
using System.Collections.Generic;
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
                        }).ToDictionary(cell => cell.ColumnId.Value)
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
                    }).ToDictionary(cell => cell.ColumnId.Value)
                }).ToDictionary(item => item.Id.Value),
                Columns = dto.Headers.Select(header => new Column
                {
                    Id = header.Id,
                    Name = header.Name
                }).ToDictionary(item => item.Id.Value)
            };
        }

        public static void ApplyFilters(this Combination combination, CombinationFilter filters)
        {
            // Filter Rows
            combination.ProjectedRows = combination.Rows
                .Select(row => row.Value)
                .Where(row => OnKeepRow(row, filters))
                .ToDictionary(row => row.Id.Value);

            // Filter Columns
            var filteredColumns = combination.Columns
                .Select(column => column.Value)
                .Where(column => OnKeepColumn(column, filters))
                .ToDictionary(column => column.Id.Value);

            List<int> projectedColumnIds = new List<int>();

            foreach (var column in combination.Columns)
            {
                // Filter Cells
                var show = combination.ProjectedRows
                    .Select(row => row.Value.Cells[column.Key])
                    .Any(cell => OnKeepCell(cell, filters));

                if (show)
                {
                    projectedColumnIds.Add(column.Key);
                }
            }

            foreach (var row in combination.ProjectedRows)
            {
                row.Value.Cells = row.Value.Cells
                    .Select(cell => cell.Value)
                    .Where(cell => projectedColumnIds.Contains(cell.ColumnId.Value))
                    .ToDictionary(cell => cell.ColumnId.Value);
            }

            // Filter Columns
            combination.ProjectedColumns = filteredColumns
                .Select(column => column.Value)
                .Where(column => projectedColumnIds.Contains(column.Id.Value))
                .ToDictionary(column => column.Id.Value);
        }

        static bool OnKeepRow(Row row, CombinationFilter filters)
        {
            return
                (filters.KeepIfIdNotNull ? row.Id.HasValue && row.Cells.Any(cell => OnKeepCell(cell.Value, filters)) : true) &&
                (!string.IsNullOrWhiteSpace(filters.KeepIfRowTitleContains) ? row.Title.Name.Contains(filters.KeepIfRowTitleContains) : true);
        }

        static bool OnKeepColumn(Column column, CombinationFilter filters)
        {
            return
                (filters.KeepIfIdNotNull ? column.Id.HasValue : true) &&
                (!string.IsNullOrWhiteSpace(filters.KeepIfColumnTitleContains) ? column.Name.Contains(filters.KeepIfColumnTitleContains) : true);
        }

        static bool OnKeepCell(Cell cell, CombinationFilter filters)
        {
            return
                (filters.KeepIfIdNotNull ? cell.ColumnId.HasValue : true) &&
                filters.KeepIfCellAllowModeNotNull ? cell.AllowMode.HasValue : true;
        }
    }
}
