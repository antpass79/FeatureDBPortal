using FeatureDBPortal.Shared;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class CombinationCell
    {
        public int? RowId { get; set; }
        public int? ColumnId { get; set; }

        public bool? Available { get; set; }
        public bool? Visible { get; set; }
        public AllowModeDTO? AllowMode { get; set; }

        public IEnumerable<CombinationItem> Items { get; set; }
    }
}
