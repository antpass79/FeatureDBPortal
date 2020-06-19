using System.Collections.Generic;

namespace FeatureDBPortal.Client.Models
{
    public class Row
    {
        public CombinationCell TitleCell { get; set; }
        public IList<CombinationCell> Cells { get; set; }
    }
}
