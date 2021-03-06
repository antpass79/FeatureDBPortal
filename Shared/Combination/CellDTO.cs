﻿using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    public class CellDTO
    {
        public string Name { get; set; }
        public bool? Available { get; set; }
        public bool? Visible { get; set; }
        public AllowModeDTO? AllowMode { get; set; }

        public int? RowId { get; set; }
        public int? ColumnId { get; set; }

        public IReadOnlyList<CellItemDTO> Items { get; set; }
    }
}
