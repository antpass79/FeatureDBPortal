using System.Collections.Generic;

namespace FeatureDBPortal.Client.Models
{
    public class Combination
    {
        public string IntersectionTitle { get; set; }

        public IDictionary<int, Column> Columns { get; set; }

        public IDictionary<int, Row> Rows { get; set; }
    }
}
