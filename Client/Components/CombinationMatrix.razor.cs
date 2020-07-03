using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Components
{
    public class CombinationMatrixDataModel : ComponentBase
    {
        [Parameter]
        public Combination Combination { get; set; }

        protected CombinationFilter Filters = new CombinationFilter() { KeepIfIdNotNull = true, KeepIfCellAllowModeNotNull = true };

        protected override Task OnParametersSetAsync()
        {
            Combination?.ApplyFilters(Filters);
            return base.OnParametersSetAsync();
        }

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

        protected void OnApplyFilters()
        {
            Combination.ApplyFilters(Filters);
        }
    }
}