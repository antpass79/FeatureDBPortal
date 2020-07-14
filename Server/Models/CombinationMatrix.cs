using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class CombinationMatrix : Dictionary<int?, CombinationRow>
    {
        public CombinationMatrix(int capacity)
            : base(capacity)
        {
        }
    }
}
