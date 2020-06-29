using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class RowDictionary : Dictionary<int?, CombinationCell>
    {
        public RowDictionary(int capacity)
            : base(capacity)
        {
        }

        public string Name { get; set; }
        public int? RowId { get; set; }
    }
}
