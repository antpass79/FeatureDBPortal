using System.Collections.Generic;

namespace FeatureDBPortal.Client.Models
{
    public class Row
    {
        public int? RowId { get; set; }
        public CombinationCell TitleCell { get; set; }
        public IEnumerable<CombinationCell> Cells { get; set; }
    }
}
