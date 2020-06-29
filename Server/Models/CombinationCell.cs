using FeatureDBPortal.Server.Utils;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class CombinationCell
    {
        public CombinationCell()
        {
            RowId = -1;
            ColumnId = -1;
            Name = string.Empty;
            Available = null;
        }

        public string Name { get; set; }
        public bool? Available { get; set; }
        public bool? Visible { get; set; }
        public AllowMode? AllowMode { get; set; }
        public IEnumerable<CombinationItem> Items { get; set; }

        public int? RowId { get; set; }
        public int? ColumnId { get; set; }
    }
}
