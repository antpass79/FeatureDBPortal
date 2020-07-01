using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FeatureDBPortal.Client.Components
{
    public class RowComponentDataModel : ComponentBase
    {
        [Parameter]
        public EventCallback<Cell> CellClick { get; set; }

        [Parameter]
        public EventCallback<Cell> CellMouseEnter { get; set; }

        [Parameter]
        public EventCallback<Cell> CellMouseLeave { get; set; }

        [Parameter]
        public Row Row { get; set; }

        protected void OnCellClick(Cell cell)
        {
            CellClick.InvokeAsync(cell);
        }

        protected void OnCellMouseEnter(Cell cell)
        {
            CellMouseEnter.InvokeAsync(cell);
        }

        protected void OnCellMouseLeave(Cell cell)
        {
            CellMouseLeave.InvokeAsync(cell);
        }
    }
}
