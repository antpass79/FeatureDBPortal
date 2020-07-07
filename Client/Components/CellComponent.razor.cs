using FeatureDBPortal.Client.Models;
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
            cell.IsSelected = !cell.IsSelected;
            CellClick.InvokeAsync(cell);
        }

        protected void OnCellMouseEnter(Cell cell)
        {
            cell.IsActive = true;
            _shouldRender = true;
            CellMouseEnter.InvokeAsync(cell);
        }

        protected void OnCellMouseLeave(Cell cell)
        {
            cell.IsActive = false;
            _shouldRender = true;
            CellMouseLeave.InvokeAsync(cell);
        }

        [Parameter]
        public bool ShouldRefresh { get; set; }

        private bool _shouldRender;
        protected override bool ShouldRender() => _shouldRender || ShouldRefresh;

        protected override void OnAfterRender(bool firstRender)
        {
            _shouldRender = false;
        }
    }
}