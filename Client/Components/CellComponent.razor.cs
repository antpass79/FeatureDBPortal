﻿using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FeatureDBPortal.Client.Components
{
    public class CellComponentDataModel : ComponentBase
    {
        [Parameter]
        public EventCallback<Cell> CellClick { get; set; }

        [Parameter]
        public EventCallback<Cell> CellMouseEnter { get; set; }

        [Parameter]
        public EventCallback<Cell> CellMouseLeave { get; set; }

        [Parameter]
        public Cell Cell { get; set; }

        protected void OnCellClick(Cell cell)
        {
            CellClick.InvokeAsync(cell);
        }

        protected void OnCellMouseEnter(Cell cell)
        {
            cell.IsHighlight = true;
            _shouldRender = true;
            CellMouseEnter.InvokeAsync(cell);
        }

        protected void OnCellMouseLeave(Cell cell)
        {
            cell.IsHighlight = false;
            _shouldRender = true;
            CellMouseLeave.InvokeAsync(cell);
        }

        private bool _shouldRender;
        protected override bool ShouldRender() => _shouldRender;

        protected override void OnAfterRender(bool firstRender)
        {
            _shouldRender = false;
        }
    }
}