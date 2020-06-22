using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    [Serializable]
    public class CombinationCellDTO
    {
        public string Name { get; set; }
        public bool? Allow { get; set; }

        public int? RowId { get; set; }
        public int? ColumnId { get; set; }

        public IEnumerable<CombinationItemDTO> Items { get; set; }
    }
}
