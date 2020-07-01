﻿using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FeatureDBPortal.Client.Components
{
    public class CombinationMatrixDataModel : ComponentBase
    {
        [Parameter]
        public Combination Combination { get; set; }

        protected void OnCellClick(Cell cell)
        {
        }

        protected void OnCellMouseEnter(Cell cell)
        {
            UpdateSelection(cell, cell.IsHighlight);
        }

        protected void OnCellMouseLeave(Cell cell)
        {
            UpdateSelection(cell, cell.IsHighlight);
        }

        private void UpdateSelection(Cell cell, bool selected)
        {
            var row = Combination.Rows[cell.RowId.Value];
            row.Title.IsHighlight = selected;

            var column = Combination.Columns[cell.ColumnId.Value];
            column.IsHighlight = selected;
        }

        RenderFragment CreateHeader() => builder =>
        {
            builder.OpenComponent<HeaderComponent>(0);
            builder.CloseComponent();
        };

        RenderFragment CreateBody() => builder =>
        {
            builder.OpenComponent<BodyComponent>(0);
            builder.CloseComponent();
        };
    }
}