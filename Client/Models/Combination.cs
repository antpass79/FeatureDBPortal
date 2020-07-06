using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Client.Models
{
    public class Combination
    {
        public Combination()
        {
        }

        public string IntersectionTitle { get; set; }

        public IDictionary<int, Column> Columns { get; set; }

        public IDictionary<int, Row> Rows { get; set; }

        IDictionary<int, Column> _projectedColumns;
        public IDictionary<int, Column> ProjectedColumns
        {
            get { return _projectedColumns == null ? Columns : _projectedColumns; }
            set { _projectedColumns = value; }
        }

        IDictionary<int, Row> _projectedRows;
        public IDictionary<int, Row> ProjectedRows
        {
            get { return _projectedRows == null ? Rows : _projectedRows; }
            set { _projectedRows = value; }
        }
    }
}
