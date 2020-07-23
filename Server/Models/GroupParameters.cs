using FeatureDBPortal.Server.Data.Models.RD;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Models
{
    public class GroupParameters
    {
        public IList<NormalRule> NormalRules { get; set; }
        public int? ProbeId { get; set; }
    }
}
