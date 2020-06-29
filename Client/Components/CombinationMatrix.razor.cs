using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace FeatureDBPortal.Client.Components
{
    public class CombinationMatrixDataModel : ComponentBase
    {
        [Parameter]
        public Combination Combination { get; set; }

        protected void OnCellMouseEnter(CombinationCell cell)
        {
            cell.IsHighlight = true;
            
            var row = Combination.Rows.Single(row => row.RowId == cell.RowId);
            row.TitleCell.IsHighlight = true;

            var header = row.Cells.Single(header => header.ColumnId == cell.ColumnId);
            header.IsHighlight = true;
        }

        protected void OnCellMouseLeave(CombinationCell cell)
        {
            cell.IsHighlight = false;

            var row = Combination.Rows.Single(row => row.RowId == cell.RowId);
            row.TitleCell.IsHighlight = false;

            var header = row.Cells.Single(header => header.ColumnId == cell.ColumnId);
            header.IsHighlight = true;
        }
    }
}