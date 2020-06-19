using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    public class RowDTO
    {
        public CombinationCellDTO TitleCell { get; set; }
        public IEnumerable<CombinationCellDTO> Cells { get; set; }
    }
}
