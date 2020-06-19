using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class CombinationItem
    {
        public CombinationItem()
        {
            RowId = -1;
            ColumnId = -1;
            ItemId = -1;
            Name = string.Empty;
        }

        public string Name { get; set; }
        public int? RowId { get; set; }
        public int? ColumnId { get; set; }
        public int? ItemId { get; set; }
    }
}
