using System.Collections.Generic;

namespace FeatureDBPortal.Client.Models
{
    public class CombinationCell
    {
        public CombinationCell()
        {
            RowId = -1;
            ColumnId = -1;
            Name = string.Empty;
            Allow = null;
        }

        public string Name { get; set; }
        public bool? Allow { get; set; }
        public string AggregateItems { get; set; }
        public IEnumerable<CombinationItem> Items { get; set; }

        public int? RowId { get; set; }
        public int? ColumnId { get; set; }
    }
}
