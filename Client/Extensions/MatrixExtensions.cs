using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Client.Extensions
{
    public static class MatrixExtensions
    {
        public static IList<RowDTO> ToRows(this MatrixDTO matrix)
        {
            return matrix.Rows.Values.Select(item => new RowDTO
            {
                TitleCell = new CellDTO { Name = item.Name },
                Cells = item.Columns.Select(innerItem => new CellDTO
                {
                    Allow = innerItem.Value.Allow
                }).ToList()
            }).ToList();
        }
    }
}
