using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace FeatureDBPortal.Client.Components
{
    public class BodyComponentDataModel : ComponentBase
    {
        [Parameter]
        public EventCallback<Cell> CellClick { get; set; }

        [Parameter]
        public EventCallback<Cell> CellMouseEnter { get; set; }

        [Parameter]
        public EventCallback<Cell> CellMouseLeave { get; set; }

        [Parameter]
        public ICollection<Row> Rows { get; set; }

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

        protected override bool ShouldRender() => false;
    }
}
