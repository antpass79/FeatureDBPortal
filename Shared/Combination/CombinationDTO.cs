using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    public class CombinationDTO
    {
        public string IntersectionTitle { get; set; }

        public IEnumerable<ColumnDTO> Columns { get; set; }

        public IEnumerable<RowDTO> Rows { get; set; }
    }
}
