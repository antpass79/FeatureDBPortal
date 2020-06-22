using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class CombinationDictionary : Dictionary<int?, RowDictionary>
    {
        public CombinationDictionary(int capacity)
            : base(capacity)
        {
        }
    }
}
