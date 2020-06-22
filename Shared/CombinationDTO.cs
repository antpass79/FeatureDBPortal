using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    [Serializable]
    public class CombinationDTO
    {
        public IEnumerable<ColumnTitleDTO> Headers { get; set; }

        public IEnumerable<RowDTO> Rows { get; set; }

        public IEnumerable<CombinationCellDTO> Cells { get; set; }
    }
}
