using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class CombinationRow : Dictionary<int?, CombinationCell>
    {
        public CombinationRow(int capacity)
            : base(capacity)
        {
        }

        public string Name { get; set; }
        public int? Id { get; set; }
    }
}
