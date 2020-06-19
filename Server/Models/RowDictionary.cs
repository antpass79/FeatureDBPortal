using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class RowDictionary : Dictionary<int?, CombinationCell>
    {
        public string Name { get; set; }
    }
}
