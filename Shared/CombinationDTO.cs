using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    public class CombinationDTO
    {
        public IEnumerable<ColumnTitleDTO> Headers { get; set; }

        public IEnumerable<RowDTO> Rows { get; set; }
    }
}
