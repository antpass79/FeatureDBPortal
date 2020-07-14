using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    public class CombinationDTO
    {
        public string IntersectionTitle { get; set; }

        public IReadOnlyList<ColumnDTO> Columns { get; set; }

        public IReadOnlyList<RowDTO> Rows { get; set; }
    }
}
