using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Components
{
    public class CombinationMatrixDataModel : ComponentBase
    {
        private const string DATA_MESSAGE_LOADING_DATA = "Loading Data...";
        private const string DATA_MESSAGE_RENDERING_DATA = "Rendering Data...";

        [Parameter]
        public bool Busy { get; set; }

        [Parameter]
        public bool ShouldRefresh { get; set; }

        [Parameter]
        public Combination Combination { get; set; }

        [Parameter]
        public CombinationFilter Filters { get; set; }

        protected string DataMessage { get; set; } = DATA_MESSAGE_LOADING_DATA;

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

        async protected Task OnApplyFilters()
        {
            ShouldRefresh = true;
            DataMessage = DATA_MESSAGE_RENDERING_DATA;
            Busy = true;

            await Task.Run(() =>
            {
                Combination?.ApplyFilters(Filters);
                StateHasChanged();
            });

            Busy = false;
            DataMessage = DATA_MESSAGE_LOADING_DATA;
            ShouldRefresh = false;

            await Task.CompletedTask;
        }
    }
}