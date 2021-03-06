﻿using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    public class RowDTO
    {
        public RowTitleDTO Title { get; set; }
        public IReadOnlyList<CellDTO> Cells { get; set; }
        public int? RowId { get; set; }
    }
}
