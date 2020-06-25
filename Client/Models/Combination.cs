﻿using System.Collections.Generic;

namespace FeatureDBPortal.Client.Models
{
    public class Combination
    {
        public IEnumerable<ColumnTitle> Headers { get; set; }

        public IEnumerable<Row> Rows { get; set; }
    }
}