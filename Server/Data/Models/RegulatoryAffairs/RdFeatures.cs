using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class RdFeatures
    {
        public RdFeatures()
        {
            RdFeatureAvailabilities = new HashSet<RdFeatureAvailabilities>();
        }

        public string FeaCode { get; set; }
        public string FeaName { get; set; }
        public string FeaCategory { get; set; }
        public DateTime? FeaTimestamp { get; set; }
        public string FeaUser { get; set; }

        public virtual ICollection<RdFeatureAvailabilities> RdFeatureAvailabilities { get; set; }
    }
}
