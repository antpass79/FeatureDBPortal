using System;

namespace FeatureDBPortal.Shared
{
    public class CellItemDTO
    {
        public CellItemDTO()
        {
            RowId = -1;
            ColumnId = -1;
            ItemId = -1;
            Name = string.Empty;
        }

        public string Name { get; set; }
        public int? RowId { get; set; }
        public int? ColumnId { get; set; }
        public int? ItemId { get; set; }
    }
}