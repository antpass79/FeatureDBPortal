using System.Collections.Generic;

namespace FeatureDBPortal.Client.Models
{
    public class Row
    {
        public int? Id { get; set; }
        public RowTitle Title { get; set; }
        public IDictionary<int, Cell> Cells { get; set; }
    }
}
